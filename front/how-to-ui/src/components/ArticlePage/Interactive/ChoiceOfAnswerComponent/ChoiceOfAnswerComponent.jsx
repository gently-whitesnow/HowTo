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
    props.interactive.userAnswers ?? []
  );
  const [initialUserAnswers, setInitialUserAnswers] = useState(
    props.interactive.userAnswers?.slice() ?? []
  );

  const [userSuccessAnswers, setUserSuccessAnswers] = useState(
    props.interactive.userSuccessAnswers ?? []
  );

  const [questions, setQuestions] = useState(props.interactive.questions ?? []);
  const [newQuestions, setNewQuestions] = useState([]);
  const [answers, setAnswers] = useState(props.interactive.answers ?? []);
  const [newAnswers, setNewAnswers] = useState([]);

  const [lineComponents, setLineComponents] = useState();
  const [newLineComponent, setNewLineComponent] = useState();

  useEffect(() => {
    setUserSuccessAnswers(
      CopyArray(props.interactive.userSuccessAnswers, userSuccessAnswers)
    );
    setLineComponents(getLineComponents());
    setNewLineComponent(getNewLineComponents());
  }, [props.isEditing, props.interactive.userSuccessAnswers]);

  useImperativeHandle(
    ref,
    () => {
      return {
        getInteractiveReplyData() {
          return {
            upsertReplyChoiceOfAnswer: {
              answers: NullToFalse(userAnswers),
            },
          };
        },
        getInteractiveData() {
          let processedQuestions = getProcessedQuestions();
          return {
            upsertChoiceOfAnswer: {
              questions: processedQuestions,
              answers: getProcessedAnswers(processedQuestions),
            },
          };
        },
        saveReplyCallback() {
          setInitialUserAnswers(CopyArray(userAnswers, initialUserAnswers));
        },
        saveCallback() {
          let processedQuestions = getProcessedQuestions();
          let processedAnswers = getProcessedAnswers(processedQuestions);
          setQuestions(processedQuestions);
          setAnswers(processedAnswers);
          setUserSuccessAnswers(
            CopyArray(processedAnswers, userSuccessAnswers)
          );
          setNewQuestions([]);
          setNewAnswers([]);
        },
      };
    },
    []
  );

  const getProcessedQuestions = () => {
    return questions
      .filter((e) => e.trim().length !== 0)
      .concat(newQuestions.filter((e) => e.trim().length !== 0));
  };

  // TODO плохой способ маппинга текста и ответов, подумать 
  const getProcessedAnswers = (processedQuestions) => {
    let ans = answers.concat(newAnswers);
    while (ans.length < processedQuestions.length) ans.push(false);

    if (ans.length > processedQuestions.length)
      ans = ans.splice(0, processedQuestions.length);

    return NullToFalse(ans);
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
