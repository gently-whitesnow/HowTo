import { observer } from "mobx-react-lite";
import { Interweave } from "interweave";
import { useState, useEffect, useCallback } from "react";
import { useStore } from "../../../store";

const MarkdownHandler = (props) => {
  const [articleContent, setArticleContent] = useState(null);

  const { articleStore } = useStore();
  const { article } = articleStore;

  useEffect(() => {
    setArticleContent(null);
    fillArticleContent();
  }, [article.fileURL]);
  
  
  const fillArticleContent = () => {
    if(article.fileURL === undefined){
      return;
    }
    
    readFileAndFillContent(article.fileURL, (mdFileString) => {
      let htmlFileString = convertToHtmlString(mdFileString);
      let domElement = convertToDomElements(htmlFileString);
      
      setArticleContent(domElement);
    });
  };

  const readFileAndFillContent = (path, callback) => {
    
    var rawFile = new XMLHttpRequest();
    rawFile.open("GET", path, false);
    rawFile.onreadystatechange = function () {
      if (rawFile.readyState === 4) {
        if (rawFile.status === 200 || rawFile.status == 0) {
          callback(rawFile.responseText);
        }
      }
    };
    rawFile.send(null);
  };
  
  const convertToHtmlString = (rawText) => {
    var md = require("markdown-it")({
      html: true,
      linkify: true,
      typographer: true,
    }).use(require("markdown-it-emoji"));

    return md.render(rawText);
  };

  const convertToDomElements = (htmlFileString) => {
    return (<Interweave content={htmlFileString} />);
  };

  return articleContent;
};

export default observer(MarkdownHandler);
