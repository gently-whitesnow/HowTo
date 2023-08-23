import { makeAutoObservable, configure } from "mobx";
import api from "../api/api";

class ArticleStore {
  constructor(rootStore) {
    this.rootStore = rootStore;
    this.stateStore = rootStore.stateStore;
    makeAutoObservable(this);
    configure({
      enforceActions: "never",
    });
  }

  articleActionError = [];

  setArticleActionError = (error) => {
    this.articleActionError = error;
  };

  article = {};

  setArticleData = (data) => {
    this.article= {
        id: data.article.id,
        courseId: data.article.courseId,
        title: data.article.title,
        status: data.article.status,
        createdAt: data.article.createdAt,
        updatedAt: data.article.updatedAt,
        author: { userId: data.article.author?.userId, name: data.article.author?.name },
        isAuthor: data.article.isAuthor,
        isViewed: data.article.isViewed,
      }
      this.setFiles(data.files)
  };

  setArticleIsViewed = (value) => {
    this.article.isViewed = value;
  }

  setFiles = (files) => {
    if(files === undefined || files.length === 0) return;
    let file = files[0];
    let fileData = atob(file);
    let fileByteArray = new Uint8Array(fileData.length);
    for (let i = 0; i < fileData.length; i++) {
      fileByteArray[i] = fileData.charCodeAt(i);
    }
    let fileMdData = new Blob([fileByteArray], { type: 'application/octet-stream' });
    this.article.fileURL = URL.createObjectURL(fileMdData);
    this.rootStore.stateStore.setIsLoading(false);
  }

  getArticle = (courseId, articleId) => {
    this.clearStore();
    this.rootStore.stateStore.setIsLoading(true);
    api
      .getArticle(courseId, articleId)
      .then(({ data }) => {
         this.setArticleData(data)
      })
      .catch((err) => {
        this.rootStore.stateStore.setIsLoading(false);

        console.error(err);
        if (err.response?.status === 401) {
          this.rootStore.stateStore.setIsAuthorized(false);
        }
        this.setArticleActionError(err.response?.data?.reason)
      });
  };

  clearStore = () => {
    this.article = {};
  }
}

export default ArticleStore;
