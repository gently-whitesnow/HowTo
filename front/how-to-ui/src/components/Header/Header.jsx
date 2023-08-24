import theme from "../../theme";
import {
  Colorimetr,
  ColorimetrWrapper,
  HeaderContent,
  HeaderWrapper,
  Title,
} from "./Header.styles";
import { observer } from "mobx-react-lite";
import { useNavigate } from "react-router";
import { useLocation } from "react-router-dom";
import Button from "../common/Button/Button";
import { useStore } from "../../store";
import { useEffect } from "react";

const Header = () => {
  const {
    colorStore,
    courseStore,
    stateStore,
    interactiveStore,
    articleStore,
  } = useStore();
  const { currentColorTheme, setColorTheme } = colorStore;
  const {
    newInteractive,
    setNewInteractive,
    addNewInteractive,
    setIsInteractiveChoise,
    isInteractiveChoise,
  } = interactiveStore;
  const { article } = articleStore;
  const { addNewArticle, id } = courseStore;
  const { isAuthorized, isNotFound, setIsNotFound, isLoading, getAuth } =
    stateStore;

  const navigate = useNavigate();
  const onClickHandler = (path) => {
    navigate(path);
  };

  const location = useLocation();
  let path = location.pathname;

  useEffect(() => {
    getAuth();
  }, []);

  useEffect(() => {
    if (!isAuthorized) {
      navigate("/auth");
    } else if ((isAuthorized && path.includes("auth")) || isNotFound) {
      setIsNotFound(false);
      navigate("/");
    }
  }, [isAuthorized, isNotFound]);

  let maybeIsCourse = path.length > 1;
  let maybeIsArticle = path.substring(1, path.length).includes("/");

  const getCurrentHeader = () => {
    if (isLoading) return null;
    if (maybeIsCourse && maybeIsArticle) {
      return getArticlePageHeader();
    } else if (maybeIsCourse) {
      return getCoursePageHeader();
    }
    return getHomePageHeader();
  };

  const getArticlePageHeader = () => {
    return (
      <>
        <Button
          onClick={() =>
            onClickHandler(path.substring(0, path.lastIndexOf("/")))
          }
          content="Назад"
        />
        <ColorimetrWrapper>
          {article.isAuthor ? (
            newInteractive || isInteractiveChoise ? (
              <Button
                onClick={() => {
                  setNewInteractive(undefined);
                  setIsInteractiveChoise(false);
                }}
                content="Сбросить"
              />
            ) : (
              <Button
                onClick={() => setIsInteractiveChoise(true)}
                content="Добавить интерактив"
              />
            )
          ) : null}

          <Colorimetr color={currentColorTheme} onClick={setColorTheme} />
        </ColorimetrWrapper>
      </>
    );
  };

  const getCoursePageHeader = () => {
    return (
      <>
        <Button
          onClick={() =>
            onClickHandler(path.substring(0, path.lastIndexOf("/")))
          }
          content="Назад"
        />
        <ColorimetrWrapper>
          {id ? (
            <Button
              onClick={() => addNewArticle()}
              content="Добавить страницу"
            />
          ) : null}

          <Colorimetr color={currentColorTheme} onClick={setColorTheme} />
        </ColorimetrWrapper>
      </>
    );
  };

  const getHomePageHeader = () => {
    return (
      <>
        <div />
        <ColorimetrWrapper>
          <Button
            onClick={() => onClickHandler("/create")}
            content="Добавить курс"
          />
          <Colorimetr color={currentColorTheme} onClick={setColorTheme} />
        </ColorimetrWrapper>
      </>
    );
  };

  return (
    <>
      <HeaderWrapper color={currentColorTheme} isLoading={isLoading}>
        <HeaderContent>{getCurrentHeader()}</HeaderContent>
      </HeaderWrapper>
    </>
  );
};

export default observer(Header);
