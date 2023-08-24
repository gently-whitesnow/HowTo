import axios from "axios";
import { camelizeKeys, decamelizeKeys , camelize} from "humps";

export class Api {
  constructor() {
    this.client = axios.create();
    this.client.defaults.baseURL = "http://45.132.18.97/gw/";
    // this.client.defaults.baseURL = "http://localhost:80/gw/";
    this.client.defaults.baseURL = "http://localhost:1999/";
    this.client.defaults.headers["Access-Control-Allow-Origin"] = "*";
    this.client.defaults.withCredentials = true;
    this.client.timeout = 3000;

    this.client.interceptors.response.use((response) => {
      if (
        response.data &&
        response.headers["content-type"].includes("application/json")
      ) {
        response.data = camelizeKeys(response.data);
      }

      return response;
    });

    // Axios middleware to convert all api requests to snake_case
    this.client.interceptors.request.use((config) => {
      const newConfig = { ...config };
      if (newConfig.headers["Content-Type"] === "multipart/form-data")
        return newConfig;

      if (config.params) {
        newConfig.params = decamelizeKeys(config.params);
      }

      if (config.data) {
        newConfig.data = decamelizeKeys(config.data);
      }
      return newConfig;
    });
  }

  clientWrapper = (method, url, data, config = {}) => {
    const clientResult = this.client[method](url, data, config);
    return clientResult;
  };

  // summary

  getSummaryCourses = () => this.clientWrapper("get", `api/summary/courses`);

  // courses

  getCourse = (id) => this.clientWrapper("get", `api/courses/${id}`);

  upsertCourse = (courseId, title, description, image) => {
    const formData = new FormData();
    if (courseId !== undefined) {
      formData.append("CourseId", courseId);
    }

    if (image !== undefined) {
      formData.append("File", image);
    }

    formData.append("Title", title);
    formData.append("Description", description);

    return this.clientWrapper("post", "api/courses", formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    });
  };
  deleteCourse = (id) => this.clientWrapper("delete", `api/courses/${id}`);

  // articles

  getArticle = (courseId, articleId) =>
    this.clientWrapper("get", `api/articles/${courseId}/${articleId}`);

  upsertArticle = (articleId, courseId, title, file) => {
    const formData = new FormData();
    if (articleId !== undefined) {
      formData.append("articleId", articleId);
    }
    if (file !== undefined) {
      formData.append("file", file);
    }
    formData.append("courseId", courseId);
    formData.append("title", title);

    return this.clientWrapper("post", "api/articles", formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    });
  };
  deleteArticle = (courseId, articleId) =>
    this.clientWrapper("delete", `api/articles/${courseId}/${articleId}`);

  // views

  postApprovedView = (courseId, articleId) =>
    this.clientWrapper("post", `api/views/approved`, {
      course_id: courseId,
      article_id: articleId,
    });

  // auth
  // http://localhost:3000/auth?userId=69550bf7-e7e1-4650-801d-e9159530decb&userName=testSanya&role=1
  getFakeAuth = (userId, userName, userRole) =>
    this.clientWrapper(
      "get",
      `api/fakeauth?userId=${userId}&userName=${userName}&userRole=${userRole}`
    );

  getAuth = () =>
    this.clientWrapper(
      "get",
      `api/auth`
    );

  // interactive

  getInteractive = (courseId, articleId) =>
    this.clientWrapper("get", `api/interactive/${courseId}/${articleId}`);

  upsertInteractive = (upsertRequest) => 
    this.clientWrapper("post", `api/interactive`, {
      interactiveId: upsertRequest.interactiveId,
      courseId: upsertRequest.courseId,
      articleId: upsertRequest.articleId,
      description: upsertRequest.description,
      upsertCheckList: upsertRequest.upsertCheckList,
      upsertChoiceOfAnswer: upsertRequest.upsertChoiceOfAnswer,
      upsertProgramWriting: upsertRequest.upsertProgramWriting,
      upsertWritingOfAnswer: upsertRequest.upsertWritingOfAnswer,
    });

  upsertInteractiveReply = (upsertRequest) => 
    this.clientWrapper("post", `api/interactive/reply`, {
      interactiveId: upsertRequest.interactiveId,
      courseId: upsertRequest.courseId,
      articleId: upsertRequest.articleId,
      upsertReplyCheckList: upsertRequest.upsertReplyCheckList,
      upsertReplyChoiceOfAnswer: upsertRequest.upsertReplyChoiceOfAnswer,
      upsertReplyProgramWriting: upsertRequest.upsertReplyProgramWriting,
      upsertReplyWritingOfAnswer: upsertRequest.upsertReplyWritingOfAnswer,
    });

  deleteInteractive = (interactiveType, interactiveId) =>
    this.clientWrapper(
      "delete",
      `api/interactive/${interactiveType}/${interactiveId}`
    );

  changeArticleStatus = (courseId, articleId, status) =>
    this.clientWrapper(
      "put",
      `api/articles`, {
        status: status,
        courseId: courseId,
        articleId: articleId,
      }
    );

  changeCourseStatus = (courseId, status) =>
    this.clientWrapper(
      "put",
      `api/courses`, {
        status: status,
        courseId: courseId
      }
    );
}
const api = new Api();

export default api;
