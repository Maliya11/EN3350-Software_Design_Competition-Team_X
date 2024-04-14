import React from 'react';
import './review.css';
import { player } from '../../assets/player';

const Review = ({ data }) => {

  const getSelectedAnswer = (questionNumber) => {
    const questionKey = `q${questionNumber}Ans`;
    const selectedAnswerIndex = player[0][questionKey]; // Get the selected answer index
    const selectedAnswer = data[questionNumber - 1]['ans' + selectedAnswerIndex]; // Get the selected answer text
    return selectedAnswer;
  };

  const getSelectedAnswerFeedback = (questionNumber) => {
    const selectedAnswerIndex = player[0]['q' + questionNumber + 'Ans'];
    return data[questionNumber - 1]['feed' + selectedAnswerIndex];
  };

  return (
    <div className='reviewContainer'>
      <h1>Questionnaire Review</h1>
      <div className="questions">
        {data.map((question, index) => (
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
