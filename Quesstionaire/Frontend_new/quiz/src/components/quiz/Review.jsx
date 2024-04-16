import React, {useState, useEffect} from 'react';
import './review.css';

const Review = ({player}) => {

  let [questions, setQuestions] = useState([]);

  useEffect(() => {
    fetch("http://16.170.233.8:8080/question/allQuestions")
            .then(res => res.json())
            .then(result => {
                setQuestions(result);
            });
    }, []);

  const getSelectedAnswer = (questionNumber) => {
    const questionKey = `q${questionNumber}_ans`;
    console.log(questionKey);
    const selectedAnswerIndex = player[questionKey]; // Get the selected answer index
    console.log("Selected answer index:", selectedAnswerIndex);
    const selectedAnswer = questions[questionNumber - 1]['ans' + selectedAnswerIndex]; // Get the selected answer text
    console.log("Selected answer:", selectedAnswer);
    return selectedAnswer;
  };

  const getSelectedAnswerFeedback = (questionNumber) => {
    const questionKey = `q${questionNumber}_ans`;
    const selectedAnswerIndex = player[questionKey];
    return questions[questionNumber - 1]['feed' + selectedAnswerIndex];
  };

  return (
    <div className='reviewContainer'>
      <h1>Questionnaire Review</h1>
      <div className="questions">
        {questions.map((question, index) => (
          <div key={index} className="questionBox">
            <div className="questionDetails">
              <h2>{index+1}.{question.q}</h2>
              <div className="options">
              {getSelectedAnswer(index + 1) !== question['ans' + question.corAns] && (
                <p>
                  <span className="incorrectIcon">&#10008;</span>
                  {getSelectedAnswer(index + 1)}
                </p>
              )}
              </div>
              <p>
                <span className="correctIcon">&#10004;</span>
                {question['ans' + question.corAns]}
              </p>
              <h4>Feedback:</h4>
              <p>{question.genFeed}</p>
              {/* <div style={{ marginTop: '10px',color: 'brown' }}> {getSelectedAnswerFeedback(index + 1)}</div> */}
              <div className='sfeedback'> {getSelectedAnswerFeedback(index + 1)}</div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Review;
