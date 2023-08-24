import { makeAutoObservable, configure } from "mobx";
import api from "../api/api";
import { setFile } from "../helpers/IOHelper";

class CourseStore {
  constructor(rootStore) {
    this.rootStore = rootStore;
    this.stateStore = rootStore.stateStore;
    makeAutoObservable(this);
    configure({
      enforceActions: "never",
    });
  }

  id = undefined;
  title = "";
  status = 0;
  description = "";
  createdAt = "";
  updatedAt = "";

  contributors = [];
  articles = [];

  image = undefined;

  setTitle = (text) => {
    this.title = text;
  };

  setDescription = (text) => {
    this.description = text;
  };

  setCourseData = (course) => {
    this.title = course.title;
    this.description = course.description;
    this.status = course.status;
    this.articles = course.articles?.map((a) => {
      return {
        id: a.id,
        courseId: a.courseId,
        title: a.title,
        status: a.status,
        createdAt: a.createdAt,
        updatedAt: a.updatedAt,
        author: { userId: a.author?.userId, name: a.author?.name },
        isAuthor: a.isAuthor,
        isViewed: a.isViewed,
      };
    });
    this.id = course.id;
    this.createdAt = course.createdAt;
    this.updatedAt = course.updatedAt;
    this.contributors = course.contributors;
    this.isAuthor = course.isAuthor;
    this.image = setFile(course.files?.shift());
  };

  setArticleData = (article) => {
    if (this.articles === undefined) this.articles = [];

    this.articles.push({
      id: article.id,
      courseId: article.courseId,
      title: article.title,
      status: article.status,
      createdAt: article.createdAt,
      updatedAt: article.updatedAt,
      author: { userId: article.author?.userId, name: article.author?.name },
      isAuthor: article.isAuthor,
    });
  };

  courseActionError = "";

  setCourseActionError = (error) => {
    this.courseActionError = error;
  };

  newArticle = undefined;

  addNewArticle = () => {
    if (this.newArticle !== undefined) {
      return;
    }
    this.newArticle = {
      title: "",
      isAuthor: true,
      isArticleEditing: true,
      isNewArticle: true,
      courseId: this.id,
    };
  };

  setNewArticle = (value) => {
    this.newArticle = value;
  };

  isCourseEditing = false;

  setCourseCreate = () => {
    this.clearStore();
    this.setIsCourseEditing(true);
    this.setIsCourseContributor(true);
  };
  setIsCourseEditing = (value) => {
    this.isCourseEditing = value;
  };

  isAuthor = false;
  setIsCourseContributor = (value) => {
    this.isAuthor = value;
  };

  getCourse = (id) => {
    this.clearStore();
    this.rootStore.stateStore.setIsLoading(true);
    api
      .getCourse(id)
      .then(({ data }) => {
        this.rootStore.stateStore.setIsLoading(false);
        this.setCourseData(data);
      })
      .catch((err) => {
        this.rootStore.stateStore.setIsLoading(false);

        console.error(err);
        if (err.response?.status === 401) {
          this.rootStore.stateStore.setIsAuthorized(false);
        }
        let course = {
          title: err.response?.data?.error,
          description: err.response?.data?.reason,
        };
        this.setCourseData(course);
      });
  };

  upsertCourse = (courseId, title, description, image, callback) => {
    this.rootStore.stateStore.setIsLoading(true);
    api
      .upsertCourse(courseId, title, description, image)
      .then(({ data }) => {
        this.rootStore.stateStore.setIsLoading(false);
        this.id = data.id;
        this.createdAt = data.createdAt;
        this.updatedAt = data.updatedAt;
        this.setIsCourseEditing(false);
        callback(data.id);
      })
      .catch((err) => {
        this.rootStore.stateStore.setIsLoading(false);
        console.error(err);
        this.setCourseActionError(err.response?.data?.reason);
        if (err.response?.status === 401) {
          this.rootStore.stateStore.setIsAuthorized(false);
        }
        callback();
      });
  };

  deleteCourse = (courseId, callback) => {
    this.rootStore.stateStore.setIsLoading(true);
    api
      .deleteCourse(courseId)
      .then(({ data }) => {
        this.rootStore.stateStore.setIsLoading(false);
        // todo redirect and cleaning
        callback(true);
      })
      .catch((err) => {
        this.rootStore.stateStore.setIsLoading(false);
        console.error(err);
        this.setCourseActionError(err.response?.data?.reason);
        if (err.response?.status === 401) {
          this.rootStore.stateStore.setIsAuthorized(false);
        }
        callback(false);
      });
  };

  upsertArticle = (
    articleId,
    courseId,
    title,
    file,
    isNewArticle,
    errorCallback
  ) => {
    if (isNewArticle && file === undefined) {
      errorCallback("Необходимо передать MD файл");
      return;
    }
    this.rootStore.stateStore.setIsLoading(true);
    api
      .upsertArticle(articleId, courseId, title, file)
      .then(({ data }) => {
        this.rootStore.stateStore.setIsLoading(false);

        if (isNewArticle) {
          this.setNewArticle(undefined);
          this.setArticleData(data);
        }
        errorCallback();
      })
      .catch((err) => {
        this.rootStore.stateStore.setIsLoading(false);
        if (err.response?.status === 401) {
          this.rootStore.stateStore.setIsAuthorized(false);
        }
        errorCallback(err.response?.data?.reason);
      });
  };

  deleteArticle = (courseId, articleId, errorCallback) => {
    this.rootStore.stateStore.setIsLoading(true);
    api
      .deleteArticle(courseId, articleId)
      .then(({ data }) => {
        this.rootStore.stateStore.setIsLoading(false);
        this.articles = this.articles.filter((obj) => obj.id !== articleId);
      })
      .catch((err) => {
        this.rootStore.stateStore.setIsLoading(false);
        console.error(err);
        if (err.response?.status === 401) {
          this.rootStore.stateStore.setIsAuthorized(false);
        }
        errorCallback(err.response?.data?.reason);
      });
  };

  changeArticleStatus = (courseId, articleId, status) => {
    this.rootStore.stateStore.setIsLoading(true);
    api
      .changeArticleStatus(courseId, articleId,status)
      .then(({ data }) => {
        this.rootStore.stateStore.setIsLoading(false);
        this.articles.forEach((a) => {
          if (a.id === articleId) {
            a.status = data.status;
            return;
          }
        });
      })
      .catch((err) => {
        this.rootStore.stateStore.setIsLoading(false);
        console.error(err);
        if (err.response?.status === 401) {
          this.rootStore.stateStore.setIsAuthorized(false);
        }
      });
  };

  changeCourseStatus = (courseId, status) => {
    this.rootStore.stateStore.setIsLoading(true);
    api
      .changeCourseStatus(courseId, status)
      .then(({ data }) => {
        this.rootStore.stateStore.setIsLoading(false);
        this.status = data.status;
      })
      .catch((err) => {
        this.rootStore.stateStore.setIsLoading(false);
        console.error(err);
        if (err.response?.status === 401) {
          this.rootStore.stateStore.setIsAuthorized(false);
        }
      });
  };

  clearStore() {
    this.id = undefined;
    this.title = "";
    this.status = 0;
    this.description = "";
    this.createdAt = "";
    this.updatedAt = "";
    this.contributors = [];
    this.articles = [];
    this.image = undefined;
    this.courseActionError = "";
    this.newArticle = undefined;
    this.isCourseEditing = false;
    this.isAuthor = false;
  }
}

export default CourseStore;
