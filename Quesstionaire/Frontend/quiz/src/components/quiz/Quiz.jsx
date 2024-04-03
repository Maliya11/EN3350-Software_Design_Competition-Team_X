import React, {useState,useRef} from 'react'
import './quiz.css'
import { data } from '../../assets/data';

const Quiz = () => {
    let [index, setIndex] = useState(0);
    let [question,setQuestion] = useState(data[index]);
    let [lock, setLock] = useState(false);
    let [score, setScore] = useState(0);
    let [result, setResult] = useState(false);
    let [submitDisabled, setSubmitDisabled] = useState(true);
    let [feedback, setFeedback] = useState(false);
    let [answer, setAnswer] = useState(0);

    let Option1 = useRef(null);
    let Option2 = useRef(null);
    let Option3 = useRef(null);
    let Option4 = useRef(null);

    let option_array = [Option1,Option2,Option3,Option4];
    let feed_array =[question.feed1,question.feed2,question.feed3,question.feed4];

    const checkAns = (e,ans) => {
        if (lock===false){
            setAnswer(ans);
            if (question.ans===ans){
                e.target.classList.add("correct");
                setSubmitDisabled(false);
                setScore(prev=>prev+1);
            }
            else{
                e.target.classList.add("wrong");
                setSubmitDisabled(false);
                option_array[question.ans-1].current.classList.add("correct");
            }
        }
        
    }

    const next = ()=>{
        if (lock===true){
            if (index === data.length -1){
                setResult(true);
                return 0;
            }
            setIndex(++index);
            setQuestion(data[index]);
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
        setQuestion(data[0]);
        setScore(0);
        setLock(false);
        setResult(false);
        setFeedback(false);
        setAnswer(0);
    }

    const submit = ()=>{
        if (submitDisabled==false){
            // const correctOptionIndex = question.ans - 1;
            // option_array[correctOptionIndex].current.classList.add("correct");
            setSubmitDisabled(true);
            setLock(true);
            setFeedback(true);
        }

    }


  return (
    <div className='conntainer'>
        <h1>Questionnaire </h1>
        {result?<></>:<>
        <button className="top-right-button" onClick={next}>Next</button>
        <hr />
        <h2>{index+1}.{question.question}</h2>
        <ul>
            <li ref={Option1} onClick={(e)=>{checkAns(e,1)}}>{question.option1}</li>
            <li ref={Option2} onClick={(e)=>{checkAns(e,2)}}>{question.option2}</li>
            <li ref={Option3} onClick={(e)=>{checkAns(e,3)}}>{question.option3}</li>
            <li ref={Option4} onClick={(e)=>{checkAns(e,4)}}>{question.option4}</li>
        </ul>
        <button onClick={submit}>Submit</button>
        {feedback?<>
        <div>
            <p>{question.feedbackg}</p>
            <p>{feed_array[answer-1]}</p>
        </div>
        </>:<></>}
        <div className='index'>{index+1} of 10 questions</div>
        </>}
        {result?<>
        <h2>You Scored {score} out of {data.length}</h2>
        <button onClick={reset}>Reset</button>
        </>:<></>}
    </div>
  )
}

export default Quiz