import { observer } from "mobx-react-lite";
import {
  CheckListComponentWrapper,
  Checkbox,
  Checkline,
} from "./CheckListComponent.styles";
import { forwardRef, useEffect, useImperativeHandle, useState } from "react";
import { BoolArrayChanged, CopyArray } from "../../../../helpers/arrayHelper";

const CheckListComponent  = forwardRef(function CheckListComponent(props, ref) {

  useImperativeHandle(ref, () => {
    return {
      getInteractiveReplyData() {
        return({
          upsertReplyCheckList: {
            clauses: userChecked,
          },
        })
      },
      getInteractiveData() {
        
      },
      saveReplyCallback() {
        setInitialChecked(CopyArray(userChecked,initialChecked));
      },
    };
  }, []);

  const [userChecked, setUserChecked] = useState(
    props.interactive.userClausesChecked ?? []
  );
  const [initialChecked, setInitialChecked] = useState(props.interactive.userClausesChecked?.slice() ?? []);

  const [checkLines, setCheckLines] = useState();

  const onClickHandler = (index) => {
    if(props.isLoading) return;
    userChecked[index] = !userChecked[index];
    setUserChecked(userChecked);
    setCheckLines(GetCheckLines());
    props.setIsChanged(BoolArrayChanged(initialChecked, userChecked));
  };

  useEffect(() => {
    setCheckLines(GetCheckLines());
  }, [props.color]);

  const GetCheckLines = () => {
    let lines = [];
    for (let index = 0; index < props.interactive.clauses.length; index++) {
      lines.push(
        <Checkline onClick={() => onClickHandler(index)} color={props.color}>
          <Checkbox
            className={(userChecked[index] ? "right" : "") + " checkbox"}
            color={props.color}
          />
          {props.interactive.clauses[index]}
        </Checkline>
      );
    }
    return lines;
  };

  return (
    <CheckListComponentWrapper>
      {checkLines}
    </CheckListComponentWrapper>
  );
});

export default observer(CheckListComponent);
