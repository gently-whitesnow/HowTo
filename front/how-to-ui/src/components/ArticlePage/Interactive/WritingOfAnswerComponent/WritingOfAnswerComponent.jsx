import { observer } from "mobx-react-lite";
import { WritingOfAnswerComponentWrapper } from "./WritingOfAnswerComponent.styles";
import { forwardRef, useEffect, useImperativeHandle, useState } from "react";
import Textarea from "../../../common/Textarea/Textarea";
import theme from "../../../../theme";

const WritingOfAnswerComponent = forwardRef(function WritingOfAnswerComponent(
  props,
  ref
) {
  const [userAnswer, setUserAnswer] = useState(
    props.interactive.userAnswer?.slice() ?? ""
  );
  const [initialUserAnswer, setInitialUserAnswer] = useState(
    props.interactive.userAnswer?.slice() ?? ""
  );

  const [answer, setAnswer] = useState(props.interactive.answer ?? "");
  const [userSuccess, setUserSuccess] = useState(props.interactive.userSuccess);

  useEffect(() => {
    setUserSuccess(props.interactive.userSuccess);
  }, [props.interactive]);

  useImperativeHandle(
    ref,
    () => {
      const getInteractiveReplyData = () => ({
        upsertReplyWritingOfAnswer: {
          answer: userAnswer,
        },
      });

      const getInteractiveData = () => ({
        upsertWritingOfAnswer: {
          answer: answer,
        },
      });

      const saveReplyCallback = () => {
        setInitialUserAnswer(userAnswer);
      };

      const saveCallback = () =>{
        setUserAnswer("");
        setInitialUserAnswer("");
        setUserSuccess(undefined);
      }

      return {
        getInteractiveReplyData,
        getInteractiveData,
        saveReplyCallback,
        saveCallback,
      };
    },
    [userAnswer, answer]
  );

  const getTextareaStateColor = () => {
    if (userSuccess === false) return theme.colors.red;
    if (userSuccess === true) return theme.colors.green;

    return theme.colors.blue;
  };

  const setUserAnswersHandler = (ans) => {
    setUserAnswer(ans);
    props.setIsChanged(ans != initialUserAnswer);
  };

  return (
    <WritingOfAnswerComponentWrapper
      isEditing={props.isEditing}
      textareaColor={getTextareaStateColor()}
    >
      {props.isEditing ? (
        <Textarea
          className="textarea-wrapper"
          value={answer}
          disabled={false}
          onChange={(e) => setAnswer(e.target.value)}
          maxLength={40}
          height={"40px"}
          fontsize={"24px"}
          placeholder={"Введите возможный ответ"}
        />
      ) : (
        <Textarea
          className="textarea-wrapper"
          value={userAnswer}
          disabled={false}
          onChange={(e) => setUserAnswersHandler(e.target.value)}
          maxLength={40}
          height={"40px"}
          fontsize={"24px"}
          placeholder={"Введите ответ"}
        />
      )}
    </WritingOfAnswerComponentWrapper>
  );
});

export default observer(WritingOfAnswerComponent);
