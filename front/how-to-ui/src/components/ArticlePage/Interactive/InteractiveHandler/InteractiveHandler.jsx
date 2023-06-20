import { observer } from "mobx-react-lite";
import {
  AddInteractiveButton,
  AddInteractiveButtonLine,
  InteractiveHandlerWrapper,
} from "./InteractiveHandler.styles";
import { useEffect, useState } from "react";
import { useStore } from "../../../../store";

const InteractiveHandler = (props) => {
  const { interactiveStore, articleStore, colorStore } = useStore();
  const { currentColorTheme } = colorStore;
  const { getInteractive, isInteractiveChoise } = interactiveStore;
  const { article } = articleStore;

  useEffect(() => {
    getInteractive(article.courseId, article.id);
  }, [article.id, article.courseId]);

  return (
    <InteractiveHandlerWrapper>
      {isInteractiveChoise ? (
        <>
          <AddInteractiveButtonLine>
            <AddInteractiveButton color={currentColorTheme}>
              Чек лист
            </AddInteractiveButton>
            <AddInteractiveButton color={currentColorTheme}>
              Выбор ответа
            </AddInteractiveButton>
          </AddInteractiveButtonLine>
          <AddInteractiveButtonLine>
            <AddInteractiveButton color={currentColorTheme}>
              Ввод ответа
            </AddInteractiveButton>
            <AddInteractiveButton color={currentColorTheme}>
              Написание кода
            </AddInteractiveButton>
          </AddInteractiveButtonLine>
        </>
      ) : null}
    </InteractiveHandlerWrapper>
  );
};

export default observer(InteractiveHandler);
