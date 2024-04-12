import React, {useState,useRef,useEffect} from 'react'
import './quiz.css'
//import { data } from '../../assets/data';

const Quiz = () => {

    let [index, setIndex] = useState(0);
    let [questions, setQuestions] = useState([]);
    let [question, setQuestion] = useState({}); //questions[index]
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
            option_array.forEach((ref) => {
                if (ref.current) {
                    ref.current.classList.remove('selected');
                }
            });
            e.currentTarget.classList.add("selected");
            // option_array.forEach((ref, i) => {
            //     if (ref.current) {
            //         ref.current.classList.remove('selected');
            //     }
            // });
            // e.current.classList.add("selected");

            // if (question.ans===ans){
            //     // e.target.classList.add("correct");
            //     setScore(prev=>prev+1);
            // }
            // else{
            //     e.target.classList.add("wrong");
            //     option_array[question.ans-1].current.classList.add("correct");
            // }
            setSubmitDisabled(false);
        }
        
    }

    const next = ()=>{
        if (lock===true){
            if (index ===questions.length -1){
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
            const Q = {"id":idNum , "selAns":answer};
            console.log(Q);
            fetch("http://localhost:8080/question/answer",{
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

    useEffect(()=>{
        fetch("http://localhost:8080/question/allQuestions")
        .then(res=>res.json())
        .then((result)=>{
            setQuestions(result);
            setQuestion(result[0]);
        }
        )
    },[])

    


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
            <p>{feed_array[answer-1]}</p>
        </div>
        </>:<></>}
        <div className='index'>{index+1} of 10 questions</div>
        </>}
        {result?<>
        <h2>You Scored {score} out of {questions.length}</h2>
        <button onClick={reset}>Reset</button>
        </>:<></>}
    </div>

  )


}

export default Quiz

