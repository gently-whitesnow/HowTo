import { observer } from "mobx-react-lite";
import {
  CheckListComponentWrapper,
  Checkbox,
  Checkline,
  NewCheckline,
} from "./CheckListComponent.styles";
import { forwardRef, useEffect, useImperativeHandle, useState } from "react";
import { BoolArrayChanged, CopyArray } from "../../../../helpers/arrayHelper";
import Textarea from "../../../common/Textarea/Textarea";

const CheckListComponent = forwardRef(function CheckListComponent(props, ref) {
  const [userChecked, setUserChecked] = useState(
    props.interactive.userClausesChecked ?? []
  );
  const [initialChecked, setInitialChecked] = useState(
    props.interactive.userClausesChecked?.slice() ?? []
  );

  const [clauses, setClauses] = useState(props.interactive.clauses ?? []);

  const [newClauses, setNewClauses] = useState([]);

  const [checkLines, setCheckLines] = useState();
  const [newCheckLines, setNewCheckLines] = useState();

  useEffect(() => {
    setCheckLines(getCheckLines());
    setNewCheckLines(getNewCheckLines());
  }, [props.color, props.isEditing]);

  useImperativeHandle(
    ref,
    () => {
      return {
        getInteractiveReplyData() {
          return {
            upsertReplyCheckList: {
              clauses: userChecked,
            },
          };
        },
        getInteractiveData() {
          return {
            upsertCheckList: {
              clauses: getProcessedClauses(),
            },
          };
        },
        saveReplyCallback() {
          setInitialChecked(CopyArray(userChecked, initialChecked));
        },
        saveCallback() {
          setClauses(getProcessedClauses());
          setNewClauses([]);
        },
      };
    },
    []
  );

  const getProcessedClauses = () => {return clauses.concat(newClauses.filter((e) => e.trim().length !== 0))}

  const onClickHandler = (index) => {
    if (props.isLoading) return;
    userChecked[index] = !userChecked[index];
    setUserChecked(userChecked);
    setCheckLines(getCheckLines());
    props.setIsChanged(BoolArrayChanged(initialChecked, userChecked));
  };

  const setClausesText = (index, clauses, setClauses, text) => {
    clauses[index] = text;
    setClauses(clauses);
  };

  const getCheckLines = () => {
    let lines = [];
    for (let index = 0; index < clauses.length; index++) {
      lines.push(
        props.isEditing ? (
          <NewCheckline>
            <Textarea
              value={clauses[index]}
              disabled={false}
              onChange={(e) => {
                setClausesText(index, clauses, setClauses, e.target.value);
                setCheckLines(getCheckLines());
              }}
              maxLength={100}
              height={"60px"}
              fontsize={"24px"}
              placeholder={"Введите пункт списка"}
            />
          </NewCheckline>
        ) : (
          <Checkline onClick={() => onClickHandler(index)} color={props.color}>
            <Checkbox
              className={(userChecked[index] ? "checked" : "") + " checkbox"}
              color={props.color}
            />
            {clauses[index]}
          </Checkline>
        )
      );
    }
    return lines;
  };

  const getNewCheckLines = () => {
    let lines = [];
    if (!props.isEditing) return lines;

    tryPushEmptyClause();

    for (let index = 0; index < newClauses.length; index++) {
      lines.push(
        <NewCheckline>
          <Textarea
            value={newClauses[index]}
            disabled={false}
            onChange={(e) => {
              setClausesText(index, newClauses, setNewClauses, e.target.value);
              setNewCheckLines(getNewCheckLines());
            }}
            maxLength={100}
            height={"60px"}
            fontsize={"24px"}
            placeholder={"Введите пункт списка"}
          />
        </NewCheckline>
      );
    }
    return lines;
  };

  const tryPushEmptyClause = () => {
    let emptyString = false;
    for (let i = 0; i < newClauses.length; i++) {
      if (newClauses[i] === "") {
        if (emptyString) {
          newClauses.splice(i, 1);
        }
        emptyString = true;
      }
    }
    if (!emptyString) newClauses.push("");
  };

  return (
    <CheckListComponentWrapper>
      {checkLines}
      {newCheckLines}
    </CheckListComponentWrapper>
  );
});

export default observer(CheckListComponent);
