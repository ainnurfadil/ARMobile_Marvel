using System;
using System.Collections.Generic;

[Serializable]
public class QuestionData
{
    public int id;
    public string descriptionText;
    public string statementQuestion;
    public string[] imageSegment;
    public string[] sentenceStructure; // Array yang berisi teks dan marker "{blank}"
    public string[] options;
    public string[] correctAnswers;
}

[Serializable]
public class QuestionList
{
    public QuestionData[] questions;
}

[Serializable]
public class WrongFeedbackData
{
    public int id;
    public string title;
    public WrongSection[] sections;
    public string button_text;
}

[Serializable]
public class WrongSection
{
    public int section_order;
    public string header;
    public string content;
    public string[] list;
    public string image_path;
}

[Serializable]
public class WrongFeedbackList
{
    public WrongFeedbackData[] feedback_wrong;
}

[Serializable]
public class CorrectFeedbackData
{
    public int id;
    public string title;
    public string compliment;
    public string correctAnswers;
}

[Serializable]
public class CorrectFeedbackList
{
    public CorrectFeedbackData[] feedback_correct;
}