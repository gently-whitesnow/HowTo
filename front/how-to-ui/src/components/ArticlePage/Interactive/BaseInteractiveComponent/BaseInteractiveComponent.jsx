import { observer } from "mobx-react-lite";
import {
  InteractiveComponentWrapper,
  InteractiveDescription,
  InteractiveLabel,
  Border,
  ComponentWrapper,
  HeadContent,
  SaveAnswerButtonWrapper,
  SaveAnswerButton,
} from "./BaseInteractiveComponent.styles";
import { InteractiveType } from "../../../../entities/InteractiveType";
import ErrorLineHandler from "../../../common/ErrorLineHandler";
import CheckListComponent from "../CheckListComponent/CheckListComponent";
import ChoiceOfAnswerComponent from "../ChoiceOfAnswerComponent/ChoiceOfAnswerComponent";
import ProgramWritingComponent from "../ProgramWritingComponent/ProgramWritingComponent";
import WritingOfAnswerComponent from "../WritingOfAnswerComponent/WritingOfAnswerComponent";
import { useRef, useState } from "react";
import EditWidget from "../../../common/EditWidget/EditWidget";
import { useStore } from "../../../../store";
import Textarea from "../../../common/Textarea/Textarea";

const BaseInteractiveComponent = (props) => {
  const componentRef = useRef(null);

  const [label, setLabel] = useState();
  const [buttonLabel, setButtonLabel] = useState();
  const [error, setError] = useState();
  const [isEditing, setIsEditing] = useState();
  const [isChanged, setIsChanged] = useState();
  const [description, setDescription] = useState(props.interactive.description);

  const { interactiveStore, stateStore } = useStore();
  const { isLoading } = stateStore;
  const { upsertInteractiveReply, upsertInteractive, setNewInteractive, deleteInteractive } =
    interactiveStore;

  const getInteractiveComponent = (interactive) => {
    if (interactive.interactiveType === InteractiveType.CheckList) {
      label ?? setLabel("Чек лист");
      buttonLabel ?? setButtonLabel("Сохранить");
      return (
        <CheckListComponent
          interactive={interactive}
          color={props.color}
          setError={setError}
          article={props.article}
          isLoading={isLoading}
          setIsChanged={setIsChanged}
          ref={componentRef}
          isEditing={isEditing}
        />
      );
    } else if (interactive.interactiveType === InteractiveType.ChoiceOfAnswer) {
      label ?? setLabel("Выбор правильного ответа");
      buttonLabel ?? setButtonLabel("Проверить ответ");
      return (
        <ChoiceOfAnswerComponent
          interactive={interactive}
          color={props.color}
          setError={setError}
          article={props.article}
          isLoading={isLoading}
          setIsChanged={setIsChanged}
          ref={componentRef}
          isEditing={isEditing}
        />
      );
    } else if (interactive.interactiveType === InteractiveType.ProgramWriting) {
      label ?? setLabel("Написание кода");
      buttonLabel ?? setButtonLabel("Запустить код");
      return (
        <ProgramWritingComponent
          interactive={interactive}
          color={props.color}
          setError={setError}
          article={props.article}
          isLoading={isLoading}
          setIsChanged={setIsChanged}
          ref={componentRef}
          isEditing={isEditing}
        />
      );
    } else if (
      interactive.interactiveType === InteractiveType.WritingOfAnswer
    ) {
      label ?? setLabel("Ввод правильного ответа");
      buttonLabel ?? setButtonLabel("Проверить ответ");
      return (
        <WritingOfAnswerComponent
          interactive={interactive}
          color={props.color}
          setError={setError}
          article={props.article}
          isLoading={isLoading}
          setIsChanged={setIsChanged}
          ref={componentRef}
          isEditing={isEditing}
        />
      );
    }
  };

  const onEditClickHandler = () => {
    setIsEditing(!isEditing);
  };
  const onSaveClickHandler = () => {
    let request = componentRef.current.getInteractiveData();
    request.interactiveId = props.interactive.id;
    request.articleId = props.article.id;
    request.courseId = props.article.courseId;
    request.description = description;
    upsertInteractive(request, props.interactive.isNewInteractive, (error) => {
      if (error) {
        setError(error);
        return;
      }
      setIsEditing(false);
      componentRef.current.saveCallback();
    });
  };

  const onDeleteClickHandler = () => {
    if (props.interactive.isNewInteractive) {
      setNewInteractive(undefined);
      return;
    }

    deleteInteractive(props.interactive.interactiveType, props.interactive.id, (error) => {
      if (error) {
        setError(error);
        return;
      }
    });
  };

  const onSaveReplyClickHandler = () => {
    if (!isChanged) return;
    let request = componentRef.current.getInteractiveReplyData();
    request.interactiveId = props.interactive.id;
    request.articleId = props.article.id;
    request.courseId = props.article.courseId;
    upsertInteractiveReply(request, (error) => {
      if (error) {
        setError(error);
        return;
      }
      setIsChanged(false);
      componentRef.current.saveReplyCallback();
    });
  };

  return (
    <ErrorLineHandler error={error} setActionError={setError}>
      <ComponentWrapper>
        <InteractiveLabel>{label}</InteractiveLabel>
        <Border>
          <HeadContent>
            {isEditing ? (
              <Textarea
                value={description}
                disabled={false}
                onChange={(e) => setDescription(e.target.value)}
                maxLength={200}
                height={"60px"}
                fontsize={"24px"}
                placeholder={"Введите описание интерактива"}
              />
            ) : (
              <InteractiveDescription>{description}</InteractiveDescription>
            )}
            {props.article.isAuthor ? (
              <EditWidget
                onEditClickHandler={onEditClickHandler}
                onSaveClickHandler={onSaveClickHandler}
                onDeleteClickHandler={onDeleteClickHandler}
                color={props.color}
                isLoading={isLoading}
                isEditing={isEditing}
              />
            ) : null}
          </HeadContent>

          <hr />
          <InteractiveComponentWrapper>
            {getInteractiveComponent(props.interactive)}
          </InteractiveComponentWrapper>
          <SaveAnswerButtonWrapper>
            {isChanged && !isLoading ? (
              <SaveAnswerButton
                color={props.color}
                onClick={onSaveReplyClickHandler}
              >
                {buttonLabel}
              </SaveAnswerButton>
            ) : null}
          </SaveAnswerButtonWrapper>
        </Border>
      </ComponentWrapper>
    </ErrorLineHandler>
  );
};

export default observer(BaseInteractiveComponent);
