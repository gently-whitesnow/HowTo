import axios from "axios";

export class Api {
  constructor() {
    this.client = axios.create();
    this.client.defaults.baseURL = "http://45.132.18.97/gw/";
    // this.client.defaults.baseURL = "http://localhost:80/gw/";
    this.client.defaults.baseURL = "http://localhost:1999/";
    this.client.defaults.headers["Access-Control-Allow-Origin"] = "*";
    this.client.defaults.withCredentials = true;
    this.client.timeout = 3000;
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

    return this.clientWrapper("post", "api/courses", formData);
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

    return this.clientWrapper("post", "api/articles", formData);
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
  // http://localhost:3000/auth?userId=69550bf7-e7e1-4650-801d-e9159530decb&userName=testSanya
  getAuth = (userId, userName) =>
    this.clientWrapper(
      "get",
      `api/fakeauth?userId=${userId}&userName=${userName}`
    );

  // interactive

  getInteractive = (courseId, articleId) =>
    this.clientWrapper("get", `api/interactive/${courseId}/${articleId}`);

  upsertInteractive = (upsertRequest) => {
    this.clientWrapper("post", `api/interactive`, {
      interactive_id: upsertRequest.interactiveId,
      course_id: upsertRequest.courseId,
      article_id: upsertRequest.articleId,
      description: upsertRequest.description,
      upsert_check_list: upsertRequest.upsertCheckList,
      upsert_choice_of_answer: upsertRequest.upsertChoiceOfAnswer,
      upsert_program_writing: upsertRequest.upsertProgramWriting,
      upsert_writing_of_answer: upsertRequest.upsertWritingOfAnswer
    });
  }

  upsertInteractiveReply = (upsertRequest) => {
    this.clientWrapper("post", `api/interactive/reply`, {
      interactive_id: upsertRequest.interactiveId,
      course_id: upsertRequest.courseId,
      article_id: upsertRequest.articleId,
      upsert_reply_check_list: upsertRequest.upsertReplyCheckList,
      upsert_reply_choice_of_answer: upsertRequest.upsertReplyChoiceOfAnswer,
      upsert_reply_program_writing: upsertRequest.upsertReplyProgramWriting,
      upsert_reply_writing_of_answer: upsertRequest.upsertReplyWritingOfAnswer
    });
  }

  deleteInteractive = (interactiveType, interactiveId) =>
    this.clientWrapper("delete", `api/interactive/${interactiveType}/${interactiveId}`);
}
const api = new Api();

export default api;
