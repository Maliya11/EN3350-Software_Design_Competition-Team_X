import React, {useState, useRef, useEffect} from 'react'
import './quiz.css'
import Review from './Review';


const Quiz = () => {
    // State variables
    let [index, setIndex] = useState(0);            // Index of the current question
    let [questions, setQuestions] = useState([]);   // Array to store all questions
    let [question, setQuestion] = useState({});     // Current question object
    let [player, setPlayer] = useState({});         // Player details

    let [lock, setLock] = useState(false);                      // Lock state to prevent multiple submissions
    let [score, setScore] = useState(0);                        // Player's score
    let [result, setResult] = useState(false);                  // Flag to indicate if the quiz is completed
    let [submitDisabled, setSubmitDisabled] = useState(true);   // Flag to disable submit button
    
    let [feedback, setFeedback] = useState(false);  // Flag to indicate if feedback is shown
    let [answer, setAnswer] = useState(0);          // User's selected answer

    let [showReview, setShowReview] = useState(false);  // Flag to toggle review mode
    
    // Refs for options
    let Option1 = useRef(null);
    let Option2 = useRef(null);
    let Option3 = useRef(null);
    let Option4 = useRef(null);

    let option_array = [Option1,Option2,Option3,Option4];

    useEffect(() => { 
        try{
            // Fetch player details
            fetch("http://localhost:8080/player/details") 
            .then(res => res.json())
            .then(playerDetails => {
                setPlayer(playerDetails);
                setScore(playerDetails.marks);
        
                // Check if all questions are completed
                if (playerDetails.completedQuestions === 10) {
                    setResult(true);
                    return;
                }
                
                else{
                    // Fetch all questions
                    return fetch("http://localhost:8080/question/allQuestions")
                    .then(res => {
                        if (!res.ok) {
                            throw new Error('Failed to fetch questions');
                        }
                        return res.json();
                    })
                    .then(result => {
                        setQuestions(result.slice(0, 10));                      // Store the questions in the state
                        setQuestion(result[playerDetails.completedQuestions]);  // Set current question
                        setIndex(playerDetails.completedQuestions);             // Set the index of the current question
                    });
                }
            })
            
        }
        catch (error) {
            console.error("Error:", error);  // Handle error state or display a message to the user    
        }    
    }, []);

    // Function to handle option selection
    const checkAns = (e,ans) => {
        if (lock===false){
            setAnswer(ans);
            option_array.forEach((ref) => {
                if (ref.current) {
                    ref.current.classList.remove('selected');
                }
            });
            e.currentTarget.classList.add("selected");
            setSubmitDisabled(false);   // Enable the submit button
        }     
    }

    // Function to handle moving to the next question
    const next = ()=>{
        if (lock===true){
            if (index === questions.length - 1){   
                setResult(true);
                return 0;
            }
            setIndex(++index);
            setQuestion(questions[index]);
            setLock(false);
            setFeedback(false);
            setAnswer(0);
            option_array.map((option)=>{
                option.current.classList.remove("wrong");
                option.current.classList.remove("correct");
                return null;
            })
        }
    }

    // Function to submit the answer
    const submit = (e) => {
        if (submitDisabled==false){

            setSubmitDisabled(true);
            setLock(true);
            setFeedback(true);

            option_array.forEach((option) => {
                option.current.classList.remove("selected");
            });

            
            option_array[question.corAns - 1].current.classList.add("correct");
            e.preventDefault();
            const idNum = index + 1;
            const Q = {"qNum" : idNum , "selAns" : answer};

            // Send the user's answer to the server
            fetch("http://localhost:8080/player/answer",{
                method:"POST",
                headers:{"Content-Type":"application/json"},
                body:JSON.stringify(Q)
            }).then(()=>{
                console.log("Answer saved") 
            })

            // Update the score if the answer is correct
            if (answer === question.corAns){
                setScore(prev=>prev+1);
            }
            else {
                option_array[answer - 1].current.classList.add("wrong");
            }
        }
    }

    // Function to review mode
    const toggleReview = () => {
        setShowReview(prevState => !prevState);
    }


  return (
    <div className='conntainer'>
        <h1>Blitz Bolt - Questionnaire</h1>

        {/* Conditional rendering based on result */}
        {result?<>
            <h2>You Scored {score} out of 10</h2>
            <h2 style={{ fontSize: '25px' }} className="center-text">Now you can play the game</h2>
            <button onClick={toggleReview}>REVIEW</button>

        </>:<>
            <hr />
            <h2>{index+1}.{question.q}</h2>
            <ul>
                <li ref={Option1} onClick={(e)=>{checkAns(e,1)}}>{question.ans1}</li>
                <li ref={Option2} onClick={(e)=>{checkAns(e,2)}}>{question.ans2}</li>
                <li ref={Option3} onClick={(e)=>{checkAns(e,3)}}>{question.ans3}</li>
                <li ref={Option4} onClick={(e)=>{checkAns(e,4)}}>{question.ans4}</li>
            </ul>
        
            {/* Conditional rendering based on feedback */}
            {feedback?<>
                <div>
                    <p>{question.genFeed}</p>
                    <div style={{ color: 'rgb(161, 78, 78)' }}>{question[`feed${answer}`]}</div>
                </div>
                <button onClick={next}>Next</button>

            </>:<>
                <button onClick={submit}>Submit</button>
            </>}

            <div className='index'>{index+1} of 10 questions</div>
        </>}

        {/* Review component */}
        {showReview && <Review/>}

    </div>
    )
}

export default Quiz