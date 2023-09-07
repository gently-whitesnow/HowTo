import { observer } from "mobx-react-lite";
import {
  ProgramWritingComponentWrapper,
  SelectorWrapper,
} from "./ProgramWritingComponent.styles";
import Textarea from "../../../common/Textarea/Textarea";
import theme from "../../../../theme";
import { forwardRef, useEffect, useImperativeHandle, useState } from "react";
import Editor from "react-simple-code-editor";
import { highlight, languages } from "prismjs/components/prism-core";
import "prismjs/components/prism-clike";
import "prismjs/components/prism-javascript";
import "prismjs/themes/prism.css";
import Selector from "../../../common/Selector/Selector";
import { LanguageTemplate } from "./LanguageTemplate";

const ProgramWritingComponent = forwardRef(function ProgramWritingComponent(
  props,
  ref
) {
  const [userCode, setUserCode] = useState(
    props.interactive.userCode ?? props.interactive.code ?? ""
  );
  const [initialUserCode, setInitialUserCode] = useState(
    props.interactive.userCode ??
      LanguageTemplate[props.interactive.selectedLanguage?.id ?? 0]
  );

  const [code, setCode] = useState(props.interactive.code ?? "");
  const [output, setOutput] = useState(props.interactive.output ?? "");
  const [userSuccess, setUserSuccess] = useState(props.interactive.userSuccess);

  const [language, setLanguage] = useState(
    props.interactive.selectedLanguage ?? props.interactive.languages[0]
  );

  useEffect(() => {
    setUserSuccess(props.interactive.userSuccess);
    setOutput(props.interactive.output);
  }, [props.interactive]);

  useImperativeHandle(
    ref,
    () => {
      const getInteractiveReplyData = () => ({
        upsertReplyProgramWriting: {
          code: userCode,
        },
      });

      const getInteractiveData = () => ({
        upsertProgramWriting: {
          code: code,
        },
      });

      const saveReplyCallback = () => {
        setInitialUserCode(userCode);
      };

      const saveCallback = () => {
        setUserCode(code);
        setInitialUserCode(code);
        setUserSuccess(undefined);
        setOutput("");
      };

      return {
        getInteractiveReplyData,
        getInteractiveData,
        saveReplyCallback,
        saveCallback,
      };
    },
    [userCode, code]
  );

  const getTextareaStateColor = () => {
    if (userSuccess === false) return theme.colors.red;
    if (userSuccess === true) return theme.colors.green;

    return theme.colors.gray;
  };

  const setUserCodeHandler = (ans) => {
    setUserCode(ans);
    props.setIsChanged(ans != initialUserCode);
  };

  const setSelectedLanguageHandler = (selectedLanguage) => {
    setLanguage(selectedLanguage);
    setUserCode(
      props.interactive.selectedLanguage?.id === selectedLanguage.id
        ? props.interactive.userCode
        : LanguageTemplate[selectedLanguage.id ?? 0]
    );
  };

  return (
    <ProgramWritingComponentWrapper
      isEditing={props.isEditing}
      textareaColor={getTextareaStateColor()}
    >
      <SelectorWrapper>
        <Selector
          elements={props.interactive.languages}
          setSelectedElementHandler={setSelectedLanguageHandler}
          selectedElement={language}
        />
      </SelectorWrapper>
      {props.isEditing ? (
        <Editor
          value={code}
          onValueChange={(code) => setCode(code)}
          highlight={(code) => highlight(code, languages.js)}
          padding={10}
          style={{
            fontFamily: '"Fira code", "Fira Mono", monospace',
            fontSize: 12,
            border: `1px solid ${theme.colors.blue}`,
            zIndex: 1,
            minHeight:"250px"
          }}
        />
      ) : (
        <>
          <Editor
            value={userCode}
            onValueChange={(code) => setUserCodeHandler(code)}
            highlight={(code) => highlight(code, languages.js)}
            padding={10}
            style={{
              fontFamily: '"Fira code", "Fira Mono", monospace',
              fontSize: 12,
              border: `1px solid ${theme.colors.blue}`,
              zIndex: 1,
              minHeight:"250px"
            }}
          />
          <Textarea
            className="output-textarea"
            value={output ?? ""}
            disabled={true}
            height={"40px"}
            fontsize={"24px"}
            placeholder={"Ответ программы"}
          />
        </>
      )}
    </ProgramWritingComponentWrapper>
  );
});

export default observer(ProgramWritingComponent);
