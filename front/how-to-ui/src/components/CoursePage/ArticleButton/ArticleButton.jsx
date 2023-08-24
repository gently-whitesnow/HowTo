import {
  ArticleButtonContent,
  ArticleButtonWrapper,
  ArticleTag,
  ArticleTitle,
  ArticleToolsWrapper,
  IconButtonsWithUploaderWrapper,
} from "./ArticleButton.styles";
import { observer } from "mobx-react-lite";
import { useNavigate } from "react-router";
import { useRef, useState } from "react";
import Textarea from "../../common/Textarea/Textarea";
import ErrorLineHandler from "../../common/ErrorLineHandler/ErrorLineHandler";
import FileUploader from "../FileUploader/FileUploader";
import { useEffect } from "react";
import EditWidget from "../../common/EditWidget/EditWidget";
import EntityTag from "../../common/EntityTag/EntityTag";
import ThumbUpIcon from "../../common/ThumbUpIcon/ThumbUpIcon";
import { EntityStatus } from "../../../entities/entityStatus";

const ArticleButton = (props) => {
  const fileInputRef = useRef(null);

  const [isArticleEditing, setIsArticleEditing] = useState(
    props.article?.isArticleEditing
  );
  const [error, setError] = useState();
  const [title, setTitle] = useState(props.article?.title);

  useEffect(() => {
    setTitle(props.article?.title);
  }, [props.article?.title]);

  const navigate = useNavigate();
  const onArticleClickHandler = () => {
    if (isArticleEditing || props.article?.id === undefined) {
      return;
    }
    navigate(`/${props.article?.courseId}/${props.article?.id}`);
  };

  const onEditClickHandler = () => {
    setIsArticleEditing(!isArticleEditing);
  };

  const onDeleteClickHandler = () => {
    if (props.article.isNewArticle) {
      props.deleteArticle();
    }

    props.deleteArticle(props.article.courseId, props.article.id, (error) => {
      setError(error);
      if (!error) setIsArticleEditing(false);
    });
  };

  const onSaveClickHandler = () => {
    props.upsertArticle(
      props.article.id,
      props.article.courseId,
      title,
      fileInputRef.current?.files[0],
      props.article.isNewArticle,
      (error) => {
        setError(error);
        if (!error) setIsArticleEditing(false);
      }
    );
  };

  return (
    <ErrorLineHandler error={error} setActionError={setError}>
      <ArticleButtonWrapper ref={props.innerRef}>
        <ArticleButtonContent
          onClick={onArticleClickHandler}
          color={props.color}
          isArticleEditing={isArticleEditing}
        >
          {isArticleEditing ? (
            <Textarea
              value={title}
              disabled={false}
              onChange={(e) => setTitle(e.target.value)}
              maxLength={100}
              height={"60px"}
              fontsize={"24px"}
              placeholder={"Введите название страницы"}
            />
          ) : (
            <ArticleTitle>
              {title}
              <EntityTag status={props.article?.status} />{" "}
              {props.article?.status === EntityStatus.Moderation && props.userRole == 1 ? (
                <ThumbUpIcon
                  courseId={props.article.courseId}
                  articleId={props.article.id}
                />
              ) : null}
            </ArticleTitle>
          )}

          <ArticleToolsWrapper>
            {props.article?.isViewed && !isArticleEditing ? (
              <ArticleTag className="read-tag">Прочитана</ArticleTag>
            ) : null}
          </ArticleToolsWrapper>
        </ArticleButtonContent>
        {props.article?.isAuthor ? (
          <>
            {isArticleEditing ? (
              <FileUploader color={props.color} fileInputRef={fileInputRef} />
            ) : null}
            <EditWidget
              onEditClickHandler={onEditClickHandler}
              onSaveClickHandler={onSaveClickHandler}
              onDeleteClickHandler={onDeleteClickHandler}
              color={props.color}
              isLoading={props.isLoading}
              isEditing={isArticleEditing}
            />
          </>
        ) : null}
      </ArticleButtonWrapper>
    </ErrorLineHandler>
  );
};

export default observer(ArticleButton);
