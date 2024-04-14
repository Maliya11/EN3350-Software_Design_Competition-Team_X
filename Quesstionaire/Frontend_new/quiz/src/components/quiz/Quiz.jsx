import React, {useState,useRef,useEffect} from 'react'
import './quiz.css'
//import { data } from '../../assets/data';
//import { player } from '../../assets/player';
import Review from './Review';


const Quiz = () => {
    let [index, setIndex] = useState(0);
    let [questions, setQuestions] = useState([]);
    let [question, setQuestion] = useState({});
    let [player, setPlayer] = useState({});

    let [lock, setLock] = useState(false);
    let [score, setScore] = useState(0);
    let [result, setResult] = useState(false);
    let [submitDisabled, setSubmitDisabled] = useState(true);
    
    let [feedback, setFeedback] = useState(false);
    let [answer, setAnswer] = useState(0);

    let [showReview, setShowReview] = useState(false);

    // let[status , setStatus] = useState(1);
    // let[plysts, setPlysts] = useState(false);
    
    let Option1 = useRef(null);
    let Option2 = useRef(null);
    let Option3 = useRef(null);
    let Option4 = useRef(null);

    let option_array = [Option1,Option2,Option3,Option4];
    let feed_array =[question.feed1,question.feed2,question.feed3,question.feed4];

    useEffect(()=>{
        fetch("http://51.20.115.232:8080/player/details")
        .then(res=>res.json())
        .then((result)=>{
            setPlayer(result);
        }
        )
    },[])

    useEffect(() => {
        // Check if it's the first question
        fetch("http://51.20.115.232:8080/question/allQuestions")
        .then(res=>res.json())
        .then((result)=>{
            setQuestions(result);
        }
        )
        if (index === 0) {
            setScore(player.marks);
            if (player.completedQuestions === 10){
                setResult(true);
                return;
            }
            else{
                setIndex(player.cQuestion);
                setQuestion(questions[index]);
            }
        }
    }, [index, player]);

    const checkAns = (e,ans) => {
        if (lock===false){
            setAnswer(ans);
            option_array.forEach((ref) => {
                if (ref.current) {
                    ref.current.classList.remove('selected');
                }
            });
            e.currentTarget.classList.add("selected");
            setSubmitDisabled(false);
        }
        
    }

    const next = ()=>{
        if (lock===true){
            if (index === questions.length -1){
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

    const reset = ()=>{
        setIndex(0);
        setQuestion(questions[0]);
        setScore(0);
        setLock(false);
        setResult(false);
        setFeedback(false);
        setAnswer(0);
        setShowReview(false);
    }

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

            fetch("http://51.20.115.232:8080/question/answer",{
                method:"POST",
                headers:{"Content-Type":"application/json"},
                body:JSON.stringify(Q)
            }).then(()=>{
                console.log("Answer saved") 
            })

            if (answer === question.corAns){
                setScore(prev=>prev+1);
            }
            else {
                option_array[answer - 1].current.classList.add("wrong");
            }
        }
    }

    const toggleReview = () => {
        setShowReview(prevState => !prevState);
    }


  return (
    <div className='conntainer'>
        <h1>Questionnaire </h1>
        {result?<></>:<>
        <button className="top-right-button" onClick={next}>Next</button>
        <hr />
        <h2>{index+1}.{question.q}</h2>
        <ul>
            <li ref={Option1} onClick={(e)=>{checkAns(e,1)}}>{question.ans1}</li>
            <li ref={Option2} onClick={(e)=>{checkAns(e,2)}}>{question.ans2}</li>
            <li ref={Option3} onClick={(e)=>{checkAns(e,3)}}>{question.ans3}</li>
            <li ref={Option4} onClick={(e)=>{checkAns(e,4)}}>{question.ans4}</li>
        </ul>
        <button onClick={submit}>Submit</button>
        {feedback?<>
        <div>
            <p>{question.genFeed}</p>
            {/* <p>{feed_array[answer-1]}</p> */}
            <div style={{ color: 'rgb(161, 78, 78)' }}>{feed_array[answer-1]}</div>
        </div>
        </>:<></>}
        <div className='index'>{index+1} of 10 questions</div>
        </>}
        {result?<>
        <h2>You Scored {score} out of {questions.length}</h2>
        <button onClick={reset}>PLAY GAME</button>
        <button onClick={toggleReview}>REVIEW</button>
        </>:<></>}
        {showReview && <Review questions={questions} />}

    </div>
    )
}
    

export default Quiz