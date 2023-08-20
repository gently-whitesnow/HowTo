import { observer } from "mobx-react-lite";
import { InteractiveListWrapper } from "./InteractiveList.styles";
import { useEffect} from "react";
import { useStore } from "../../../../store";
import AddInteractiveGrid from "../AddInteractiveGrid/AddInteractiveGrid";
import BaseComponent from "../BaseInteractiveComponent/BaseInteractiveComponent";

const InteractiveList = (props) => {
  const { interactiveStore, articleStore, colorStore } = useStore();
  const { currentColorTheme } = colorStore;
  const { getInteractive, interactive, newInteractive} =
    interactiveStore;
  const { article } = articleStore;

  useEffect(() => {
    if (!article.id) return;
    getInteractive(article.courseId, article.id);
  }, [article.id, article.courseId]);

  return (
    <InteractiveListWrapper>
      {interactive?.map((interactive) => {
        return <BaseComponent interactive={interactive} color={currentColorTheme} article={article}/>;
      })}
      {newInteractive !== undefined ? (
        <BaseComponent interactive={newInteractive} color={currentColorTheme} article={article}/>
      ) : null}
      <AddInteractiveGrid />
    </InteractiveListWrapper>
  );
};

export default observer(InteractiveList);
