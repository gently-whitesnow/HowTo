import { observer } from "mobx-react-lite";
import {
  AddInteractiveButton,
  AddInteractiveButtonLine,
  AddInteractiveGridWrapper,
} from "./AddInteractiveGrid.styles";
import { useEffect, useRef, useState } from "react";
import { useStore } from "../../../../store";
import { InteractiveType } from "../../../../entities/InteractiveType";

const AddInteractiveGrid = () => {
  const newAddInteractiveChoiseRef = useRef();
  const { interactiveStore, articleStore, colorStore } = useStore();
  const { currentColorTheme } = colorStore;
  const { isInteractiveChoise, addNewInteractive, setIsInteractiveChoise } =
    interactiveStore;
  const { article } = articleStore;

  useEffect(() => {
    if (isInteractiveChoise)
      newAddInteractiveChoiseRef.current?.scrollIntoView({
        behavior: "smooth",
      });
  }, [isInteractiveChoise]);

  const onInteractiveSelect = (interactiveType) => {
    setIsInteractiveChoise(false);
    addNewInteractive(article.id, article.courseId, interactiveType);
  };

  return (
    <AddInteractiveGridWrapper ref={newAddInteractiveChoiseRef}>
      {isInteractiveChoise ? (
        <>
          <AddInteractiveButtonLine>
            <AddInteractiveButton
              color={currentColorTheme}
              onClick={() => onInteractiveSelect(InteractiveType.CheckList)}
            >
              Чек лист
            </AddInteractiveButton>
            <AddInteractiveButton
              color={currentColorTheme}
              onClick={() =>
                onInteractiveSelect(InteractiveType.ChoiceOfAnswer)
              }
            >
              Выбор ответа
            </AddInteractiveButton>
          </AddInteractiveButtonLine>
          <AddInteractiveButtonLine>
            <AddInteractiveButton
              color={currentColorTheme}
              onClick={() =>
                onInteractiveSelect(InteractiveType.WritingOfAnswer)
              }
            >
              Ввод ответа
            </AddInteractiveButton>
            <AddInteractiveButton
              color={currentColorTheme}
              onClick={() =>
                onInteractiveSelect(InteractiveType.ProgramWriting)
              }
            >
              Написание кода
            </AddInteractiveButton>
          </AddInteractiveButtonLine>
        </>
      ) : null}
    </AddInteractiveGridWrapper>
  );
};

export default observer(AddInteractiveGrid);
