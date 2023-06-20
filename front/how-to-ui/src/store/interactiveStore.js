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
  lastInteractive = undefined;
  newInteractive = undefined;

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

  addInteractiveData = (interactiveType, interactiveData) => {
    if (this.interactive === undefined) this.interactive = [];

    let data = {}
    if(interactiveType === InteractiveType.CheckList)
    {
      data = interactiveData.CheckList
    }
    else if(interactiveType === InteractiveType.ChoiceOfAnswer)
    {
      data = interactiveData.ChoiceOfAnswer
    }
    else if(interactiveType === InteractiveType.ProgramWriting)
    {
      data = interactiveData.ProgramWriting
    }
    else if(interactiveType === InteractiveType.WritingOfAnswer)
    {
      data = interactiveData.WritingOfAnswer
    }
    let ineractiveEntity = {
      interactiveId: data.id,
      courseId: data.course_id,
      description: data.description,
      isInteractiveEditing: false,
    }

    this.interactive.push();
  };

  getInteractive = (courseId, articleId) => {
    this.clearStore();
    this.rootStore.stateStore.setIsLoading(true);
    api
      .getInteractive(courseId, articleId)
      .then(({ data }) => {
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

  upsertInteractive = (
    interactiveId,
    articleId,
    courseId,
    description,
    isNewInteractive,
    errorCallback
  ) => {
    this.rootStore.stateStore.setIsLoading(true);
    api
      .upsertInteractive(
        articleId,
        courseId,
        interactiveId,
        description,
        TODO_ELEMENTS
      )
      .then(({ data }) => {
        this.rootStore.stateStore.setIsLoading(false);

        if (isNewInteractive) {
          this.setNewArticle(undefined);
          // TODO
          this.interactive.push(data.TODO);
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

  upsertInteractiveReply = (
    interactiveId,
    articleId,
    courseId,
    errorCallback
  ) => {
    this.rootStore.stateStore.setIsLoading(true);
    api
      .upsertInteractiveReply(articleId, courseId, interactiveId, TODO_ELEMENTS)
      .then(({ data }) => {
        this.rootStore.stateStore.setIsLoading(false);

        // TODO
        this.lastInteractive.push(data.TODO);
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
        // TODO this.interactive remove element
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

export const InteractiveType = Enum({
  CheckList: 0,
  ChoiceOfAnswer: 1,
  ProgramWriting: 2,
  WritingOfAnswer: 3,
});
