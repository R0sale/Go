import React, { useState } from "react";
import { deleteUser, signInWithPopup} from "firebase/auth";
import { auth, googleProvider } from "../firebase";
import bgImage from '../assets/worldmap.jpg';
import { useNavigate } from "react-router-dom";
import * as z from "zod";
import { config } from "../config";

const GoogleSignInPage: React.FC = () => {
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [userName, setUserName] = useState('');
    const navigate = useNavigate();

    const validate = (user: object) => {
        const User = z.object({
            userName: z.string().min(4).max(20),
            firstName: z.string().min(4).max(20),
            lastName: z.string().min(4).max(20)
        })

        User.parse(user);
    }

    const login = async () => {
        try {
            validate({firstName: `${firstName}`,
                    lastName: `${lastName}`,
                    userName: `${userName}`});

            const result = await signInWithPopup(auth, googleProvider);
            const token = await result.user.getIdToken();

            const response = await fetch(config.SIGN_UP_VIA_GOOGLE_URL, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-type': 'application/json'
                },
                body: JSON.stringify({
                    firstName: `${firstName}`,
                    lastName: `${lastName}`,
                    userName: `${userName}`
                })
            })

            if (response.ok)
                navigate('/');
            else {
                deleteUser(result.user);
                alert('Something went wrong!');
            }
        } catch (error) {
            if (error instanceof Error) {
                
                alert("Error: " + error.message);
            } else {
                alert("Unknown error");
            }
        }
    };

    const goToSignUp = () => {
        navigate('/signup');
    };

    return (
        <div style={{backgroundImage: `url(${bgImage})`, backgroundRepeat: 'space repeat'}} className="w-screen h-screen overflow-hidden">
            <div className="h-screen w-1/3 bg-white m-auto">
                <div>
                    <label className="block m-2 font-semibold">FirstName</label>
                    <input placeholder="Enter your FirstName" type="text" value={firstName} className="w-full border-2 p-1 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {setFirstName(e.target.value)}}></input>

                    <label className="block m-2 font-semibold">LastName</label>
                    <input placeholder="Enter your LastName" type="text" value={lastName} className="w-full p-1 border-2 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {setLastName(e.target.value)}}></input>

                    <label className="block m-2 font-semibold">UserName</label>
                    <input placeholder="Enter your userName" type="text" value={userName} className="w-full p-1 border-2 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {setUserName(e.target.value)}}></input>

                    <button className="m-2 w-30 border-2 border-gray-300" onClick={login}>Sign Up</button>
                </div>
                <div className="flex justify-end m-2 mt-4">
                    <button className="w-30 border-2 border-gray-300" onClick={goToSignUp}>Sign Up Manually</button>
                </div>
            </div>
        </div>);
};

export default GoogleSignInPage;