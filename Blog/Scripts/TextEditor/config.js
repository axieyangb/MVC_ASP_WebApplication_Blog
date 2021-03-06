﻿CKEDITOR.editorConfig = function (config) {

    config.contentsCss = '/Content/css/fonts.css';
    config.font_names = '娃娃体/ChineseCute;' + '二分之一爱情/aiqing;' + "徐静蕾体/xujinglei;" + "圆体/yuanti;" + 'Arial/Arial, Helvetica, sans-serif;' +
'Times New Roman/Times New Roman, Times, serif;' +'Initial/initial;'+
'Verdana;';
    config.extraPlugins = 'mathjax,link,font,justify,smiley,panelbutton,colorbutton';

    config.mathJaxLib = 'http://cdn.mathjax.org/mathjax/2.2-latest/MathJax.js?config=TeX-AMS_HTML';
    config.removeButtons = 'Underline,Subscript,Superscript,About';
    config.skin = 'kama';
};



