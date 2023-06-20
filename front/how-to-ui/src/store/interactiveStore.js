import { makeAutoObservable, configure } from "mobx";
import api from "../api/api";
import { camelize } from "../helpers/caseHelper";

class InteractiveStore {
  constructor(rootStore) {
    this.rootStore = rootStore;
    this.stateStore = rootStore.stateStore;
    makeAutoObservable(this);
    configure({
      enforceActions: "never",
    });
  }

  interactive = undefined;
  lastInteractive = undefined;
  newInteractive = undefined;

  isInteractiveChoise = false;

  setIsInteractiveChoise = (data) => {
    this.isInteractiveChoise = data;
  };

  setNewInteractive = (data) => {
    this.newInteractive = data;
  };

  addNewInteractive = (articleId, courseId, interactiveType) => {
    if (this.newInteractive !== undefined) {
      return;
    }
    this.newInteractive = {
      description: "",
      isInteractiveEditing: true,
      isNewInteractive: true,
      articleId: articleId,
      courseId: courseId,
      interactiveType: interactiveType,
    };
  };

  addInteractiveData = (interactiveData) => {
    if (this.interactive === undefined) this.interactive = [];

    let data = camelize(
      interactiveData.check_list ??
        interactiveData.choice_of_answer ??
        interactiveData.program_writing ??
        interactiveData.writing_of_answer
    );
    this.interactive.push(data);
  };

  addLastInteractiveData = (lastInteractiveData) => {
    if (this.lastInteractive === undefined) this.lastInteractive = [];

    var lastData =
      lastInteractiveData.last_check_list ??
      lastInteractiveData.last_choice_of_answer ??
      lastInteractiveData.last_program_writing ??
      lastInteractiveData.last_writing_of_answer;
    lastData = camelize(lastData);

    let elementIndex = this.lastInteractive.findIndex(
      (i) =>
        lastData.interactiveId === i.interactiveId &&
        lastData.CourseId === i.CourseId &&
        lastData.ArticleId === i.ArticleId
    );
    if (elementIndex === -1) {
      this.lastInteractive.push(lastData);
    } else {
      this.lastInteractive[elementIndex] = lastData;
    }
  };

  getInteractive = (courseId, articleId) => {
    this.clearStore();
    this.rootStore.stateStore.setIsLoading(true);
    api
      .getInteractive(courseId, articleId)
      .then(({ data }) => {
        this.rootStore.stateStore.setIsLoading(false);
        this.interactive = data.interactive;
        this.lastInteractive = data.lastInteractive;
      })
      .catch((err) => {
        this.rootStore.stateStore.setIsLoading(false);

        console.error(err);
        if (err.response?.status === 401) {
          this.rootStore.stateStore.setIsAuthorized(false);
        }
      });
  };

  upsertInteractive = (upsertRequest, isNewInteractive, errorCallback) => {
    this.rootStore.stateStore.setIsLoading(true);
    api
      .upsertInteractive(upsertRequest)
      .then(({ data }) => {
        console.log(data);
        this.rootStore.stateStore.setIsLoading(false);

        if (isNewInteractive) {
          this.addNewInteractive(undefined);
          this.addInteractiveData(data);
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

  upsertInteractiveReply = (upsertRequest, errorCallback) => {
    this.rootStore.stateStore.setIsLoading(true);
    api
      .upsertInteractiveReply(upsertRequest)
      .then(({ data }) => {
        console.log(data);
        this.rootStore.stateStore.setIsLoading(false);

        this.addLastInteractiveData(data);
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

  deleteInteractive = (interactiveType, interactiveId, errorCallback) => {
    this.rootStore.stateStore.setIsLoading(true);
    api
      .deleteInteractive(interactiveType, interactiveId)
      .then(({ data }) => {
        this.rootStore.stateStore.setIsLoading(false);
        this.interactive = this.interactive.filter(
          (obj) => obj.interactiveId !== data.interactive_id
        );
        this.lastInteractive = this.lastInteractive.filter(
          (obj) => obj.interactiveId !== data.interactive_id
        );
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

  clearStore = () => {
    this.newInteractive = undefined;
    this.lastInteractive = undefined;
    this.interactive = undefined;
  };
}

export default InteractiveStore;
