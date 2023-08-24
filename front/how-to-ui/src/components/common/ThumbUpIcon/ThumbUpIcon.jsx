import { ThumbUpIconWrapper } from "./ThumbUpIcon.styles";
import theme from "../../../theme";
import { ReactComponent as IconCheck } from "../../../icons/thumb-up24.svg";
import { useStore } from "../../../store";
import { EntityStatus } from "../../../entities/entityStatus";

const ThumbUpIcon = (props) => {
  const { courseStore, stateStore } = useStore();
  const { isLoading } = stateStore;

  const { changeCourseStatus: moderateCourse, changeArticleStatus: moderateArticle } = courseStore;

  const onClickHandler = (e) => {
    e.stopPropagation();
    if (props.articleId !== undefined && props.courseId !== undefined)
      moderateArticle(props.courseId, props.articleId, EntityStatus.Published);
    else if (props.courseId !== undefined)
      moderateCourse(props.courseId, EntityStatus.Published);
  };

  return (
    <ThumbUpIconWrapper
      className="thimb-up-icon"
      onClick={onClickHandler}
      size={props.size}
      color={theme.colors.yellow}
      disabled={isLoading}
    >
      <IconCheck />
    </ThumbUpIconWrapper>
  );
};

export default ThumbUpIcon;
