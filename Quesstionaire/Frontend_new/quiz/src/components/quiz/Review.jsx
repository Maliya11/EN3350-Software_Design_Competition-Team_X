import React, {useState, useEffect} from 'react';
import './review.css'; // Importing CSS file for styling

const Review = ({}) => {

  // State variable to store questions
  let [questions, setQuestions] = useState([]);
  let [player, setPlayer] = useState({});

  useEffect(() => {
    try{
      // Fetch all questions from the server
      fetch("http://13.60.31.79:8080/question/allQuestions")
            .then(res => {
              if (!res.ok) {
                throw new Error('Failed to fetch questions');
              }
              return res.json();
            })
            .then(result => {
                setQuestions(result); // Store fetched questions in state
            });
    }
    catch (error) {
      console.error("Error:", error); // Handle error state or display a message to the user
    }
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

  // Function to get the feedback for the selected answer
  const getSelectedAnswerFeedback = (questionNumber) => {
    const questionKey = `q${questionNumber}_ans`;
    const selectedAnswerIndex = player[questionKey];
    return questions[questionNumber - 1]['feed' + selectedAnswerIndex];
  };

  return (
    <div className='reviewContainer'>
      <h1>Questionnaire Review</h1>
      <div className="questions">
        {/* Map through questions array to display each question */}
        {questions.map((question, index) => (
          <div key={index} className="questionBox">
            <div className="questionDetails">
              {/* Display question number and text */}
              <h2>{index+1}.{question.q}</h2>
              <div className="options">
              {/* Display selected answer if incorrect */}
              {getSelectedAnswer(index + 1) !== question['ans' + question.corAns] && (
                <p>
                  <span className="incorrectIcon">&#10008;</span>
                  {getSelectedAnswer(index + 1)}
                </p>
              )}
              </div>
              {/* Display correct answer */}
              <p>
                <span className="correctIcon">&#10004;</span>
                {question['ans' + question.corAns]}
              </p>
              <h4>Feedback:</h4>
              {/* Display general feedback for the question */}
              <p>{question.genFeed}</p>
              {/* Display specific feedback for the question */}
              <div className='sfeedback'> {getSelectedAnswerFeedback(index + 1)}</div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Review;
