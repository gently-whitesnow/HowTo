import { observer } from "mobx-react-lite";
import { ChoiceOfAnswerComponentWrapper } from "./ChoiceOfAnswerComponent.styles";
import { forwardRef, useEffect, useImperativeHandle, useState } from "react";
import {
  AllUndefined,
  BoolArrayChanged,
  CopyArray,
  NullToFalse,
} from "../../../../helpers/arrayHelper";
import {
  Checkbox,
  Checkline,
} from "../ChoiceOfAnswerComponent/ChoiceOfAnswerComponent.styles";
import Textarea from "../../../common/Textarea/Textarea";

const ChoiceOfAnswerComponent = forwardRef(function ChoiceOfAnswerComponent(
  props,
  ref
) {
  const [userAnswers, setUserAnswers] = useState(
    props.interactive.userAnswers ?? [],
  );
  const [initialUserAnswers, setInitialUserAnswers] = useState(
    props.interactive.userAnswers?.slice()  ?? [],
  );

  const [userSuccessAnswers, setUserSuccessAnswers] = useState(
    props.interactive.userSuccessAnswers ?? [],
  );

  const [questions, setQuestions] = useState(props.interactive.questions ?? []);
  const [newQuestions, setNewQuestions] = useState([]);
  const [answers, setAnswers] = useState(props.interactive.answers ?? []);
  const [newAnswers, setNewAnswers] = useState([]);

  const [lineComponents, setLineComponents] = useState();
  const [newLineComponent, setNewLineComponent] = useState();

  useEffect(() => {
    // TODO CopyArray костыль, от которого не могу отказаться
    setUserSuccessAnswers(CopyArray(props.interactive.userSuccessAnswers ?? [], userSuccessAnswers));
    setLineComponents(getLineComponents());
    setNewLineComponent(getNewLineComponents());
  }, [props.isEditing, props.interactive.userSuccessAnswers]);

  useImperativeHandle(
    ref,
    () => {
      const getInteractiveReplyData = () => {
        return {
          upsertReplyChoiceOfAnswer: {
            answers: NullToFalse(userAnswers),
          },
        };
      };
      const getInteractiveData = () => {
        let processedData = getProcessedData();
        return {
          upsertChoiceOfAnswer: {
            questions: processedData.questionsList,
            answers: processedData.answersList,
          },
        };
      };
      const saveReplyCallback = () => {
        setInitialUserAnswers([...userAnswers]);
      };
      const saveCallback = () => {
        let processedData = getProcessedData();
        setQuestions(processedData.questionsList);
        setAnswers(processedData.answersList);
        // TODO Если смотреть через консоль то методы не отрабатывают должным образом
        setUserSuccessAnswers([]);
        setNewQuestions([]);
        setNewAnswers([]);
        setUserAnswers([]);
      };
      return {
        getInteractiveReplyData,
        getInteractiveData,
        saveReplyCallback,
        saveCallback,
      };
    },
    [
      userAnswers,
      userSuccessAnswers,
      questions,
      newQuestions,
      answers,
      newAnswers,
    ]
  );

  const getProcessedData = () => {
    let questionsList = [];
    let answersList = [];
    let allQuestions = questions.concat(newQuestions);
    let allAnswers = answers.concat(newAnswers);
    for (let index = 0; index < allQuestions.length; index++) {
      if (allQuestions[index].trim().length === 0) continue;

      questionsList.push(allQuestions[index]);
      answersList.push(allAnswers[index] ?? false);
    }
    return { questionsList, answersList };
  };

  const onClickHandler = (index) => {
    if (props.isLoading) return;
    userAnswers[index] = !userAnswers[index];
    setUserAnswers(userAnswers);
    setUserSuccessAnswers(AllUndefined(userSuccessAnswers));
    setLineComponents(getLineComponents());
    props.setIsChanged(BoolArrayChanged(initialUserAnswers, userAnswers));
  };

  const onEditableCheckBoxClickHandler = (
    index,
    answers,
    setAnswers,
    setLine,
    getLine
  ) => {
    if (answers.length <= index) answers.push(false);
    answers[index] = !answers[index];
    setAnswers(answers);
    setLine(getLine());
  };

  const setQuestionText = (index, question, setQuestion, text) => {
    question[index] = text;
    setQuestion(question);
  };

  const getLineComponents = () => {
    let lines = [];
    for (let index = 0; index < questions.length; index++) {
      lines.push(
        props.isEditing ? (
          <Checkline>
            <Checkbox
              onClick={() =>
                onEditableCheckBoxClickHandler(
                  index,
                  answers,
                  setAnswers,
                  setLineComponents,
                  getLineComponents
                )
              }
              className={
                answers[index]
                  ? "true-check checked"
                  : "true-check" + " checkbox"
              }
            />
            <Textarea
              value={questions[index]}
              disabled={false}
              onChange={(e) => {
                setQuestionText(index, questions, setQuestions, e.target.value);
                setLineComponents(getLineComponents());
              }}
              maxLength={100}
              height={"60px"}
              fontsize={"24px"}
              placeholder={"Введите возможный ответ"}
            />
          </Checkline>
        ) : (
          <Checkline onClick={() => onClickHandler(index)} color={props.color}>
            <Checkbox
              className={
                checkBoxState(
                  userAnswers[index],
                  userSuccessAnswers ? userSuccessAnswers[index] : undefined
                ) + " checkbox"
              }
              color={props.color}
            />
            {questions[index]}
          </Checkline>
        )
      );
    }
    return lines;
  };

  const checkBoxState = (userAnswer, userSuccessAnswer) => {
    let classString = "";
    if (userAnswer === true) classString += "checked";
    if (userSuccessAnswer === true) classString += " true-check";
    else if (userSuccessAnswer === false) classString += " false-check";
    return classString;
  };

  const getNewLineComponents = () => {
    let lines = [];
    if (!props.isEditing) return lines;

    tryPushEmptyQuestion();

    for (let index = 0; index < newQuestions.length; index++) {
      lines.push(
        <Checkline>
          <Checkbox
            onClick={() =>
              onEditableCheckBoxClickHandler(
                index,
                newAnswers,
                setNewAnswers,
                setNewLineComponent,
                getNewLineComponents
              )
            }
            className={
              newAnswers[index]
                ? "true-check checked"
                : "true-check" + " checkbox"
            }
          />
          <Textarea
            value={newQuestions[index]}
            disabled={false}
            onChange={(e) => {
              setQuestionText(
                index,
                newQuestions,
                setNewQuestions,
                e.target.value
              );
              setNewLineComponent(getNewLineComponents());
            }}
            maxLength={100}
            height={"60px"}
            fontsize={"24px"}
            placeholder={"Введите возможный ответ"}
          />
        </Checkline>
      );
    }
    return lines;
  };

  const tryPushEmptyQuestion = () => {
    let emptyString = false;
    for (let i = 0; i < newQuestions.length; i++) {
      if (newQuestions[i] === "") {
        if (emptyString) {
          newQuestions.splice(i, 1);
        }
        emptyString = true;
      }
    }
    if (!emptyString) newQuestions.push("");
  };

  return (
    <ChoiceOfAnswerComponentWrapper>
      {lineComponents}
      {newLineComponent}
    </ChoiceOfAnswerComponentWrapper>
  );
});

export default observer(ChoiceOfAnswerComponent);
