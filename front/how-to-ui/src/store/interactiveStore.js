import { makeAutoObservable, configure } from "mobx";
import api from "../api/api";

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

  initInteractiveData = (interactiveData) => {
    this.interactive = [];
    if (interactiveData.checkList)
      this.interactive = this.interactive.concat(interactiveData.checkList);
    if (interactiveData.choiceOfAnswer)
      this.interactive = this.interactive.concat(
        interactiveData.choiceOfAnswer
      );
    if (interactiveData.programWriting)
      this.interactive = this.interactive.concat(
        interactiveData.programWriting
      );
    if (interactiveData.writingOfAnswer)
      this.interactive = this.interactive.concat(
        interactiveData.writingOfAnswer
      );
  };

  addInteractiveData = (interactiveData) => {
    if (this.interactive === undefined) this.interactive = [];

    let data =
      interactiveData.checkList ??
      interactiveData.choiceOfAnswer ??
      interactiveData.programWriting ??
      interactiveData.writingOfAnswer;

    this.interactive.push(data);
  };

  upsertInteractiveReplyHandler = (interactiveReplyData) => {
    if (interactiveReplyData === undefined) return;

    let data =
      interactiveReplyData.checkList ??
      interactiveReplyData.choiceOfAnswer ??
      interactiveReplyData.programWriting ??
      interactiveReplyData.writingOfAnswer;

    this.interactive = this.interactive.map((interactive) =>
      interactive.id === data.id &&
      interactive.interactiveType === data.interactiveType
        ? data
        : interactive
    );
  };

  getInteractive = (courseId, articleId) => {
    this.clearStore();
    this.rootStore.stateStore.setIsLoading(true);
    api
      .getInteractive(courseId, articleId)
      .then(({ data }) => {
        this.rootStore.stateStore.setIsLoading(false);
        this.initInteractiveData(data);
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
        this.rootStore.stateStore.setIsLoading(false);

        if (isNewInteractive) {
          this.setNewInteractive(undefined);
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

  upsertInteractiveReply = (upsertRequest, callback) => {
    this.rootStore.stateStore.setIsLoading(true);
    api
      .upsertInteractiveReply(upsertRequest)
      .then(({ data }) => {
        this.upsertInteractiveReplyHandler(data);
        this.rootStore.stateStore.setIsLoading(false);
        callback();
      })
      .catch((err) => {
        console.error(err);
        this.rootStore.stateStore.setIsLoading(false);
        if (err.response?.status === 401) {
          this.rootStore.stateStore.setIsAuthorized(false);
        }
        callback(err.response?.data?.reason ?? err);
      });
  };

  deleteInteractive = (interactiveType, interactiveId, errorCallback) => {
    this.rootStore.stateStore.setIsLoading(true);
    api
      .deleteInteractive(interactiveType, interactiveId)
      .then(({ data }) => {
        this.rootStore.stateStore.setIsLoading(false);
        this.interactive = this.interactive.filter(
          (obj) => obj.id !== interactiveId
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
    this.interactive = undefined;
  };
}

export default InteractiveStore;
