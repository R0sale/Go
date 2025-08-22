import React from "react";
import KeysImage from '../assets/keys.png'
import { MapPin, Bus, Camera, Star, Text, Megaphone } from 'lucide-react';
import { Link, useNavigate } from "react-router-dom";
import { auth } from "../firebase";
import { signOut } from "firebase/auth";
import { useEffect, useState } from 'react';

const menu = [
    {label: 'Rate Facility', icon: <Star className="w-7 h-7"/>},
    {label: 'Change Map', icon: <MapPin className="w-7 h-7" />},
    {label: 'Report', icon: <Text className="w-7 h-7" />},
    {label: 'Advert', icon: <Megaphone className="w-7 h-7" />},
];

interface MenuProps {
    menuState: boolean;
}

const Menu: React.FC<MenuProps> = ({menuState}) => {
    const navigate = useNavigate();
    const [userName, setUserName] = useState('');
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        setLoading(true);

        const unsubscribe = auth.onAuthStateChanged(async (user) => {
            if (user)
            {
                const tokenResult = (await user.getIdTokenResult(true)).claims;
                setUserName(tokenResult.userName);
            }

            setLoading(false);
        });

        

        return () => unsubscribe();
    }, [menuState]);

    if (loading) {
        return (<div className="flex h-full w-full">
            <span className="loader m-auto"></span>
        </div>);
    }

    return (
        <div className="mt-10">
            {auth.currentUser == null ? <div className="p-4 pt-0">
                <div className='font-bold text-lg'>
                    Log in your account
                </div>

                <div className="w-48 inline-block">
                    You could comment facilities, where you've been
                </div>

                <img src={KeysImage} alt="Keys" className="inline w-52 h-48"/>

                <Link to="/login" className="rounded-2xl no-underline text-white bg-blue-600 p-4 mt-2 w-24 text-center ml-0 hover:opacity-75">Log in</Link>
            </div> : <div className="p-4 pt-0">
                <p className="font-bold m-2 text-left">Your current account email: </p>
                <div className="border-2 border-black rounded-2xl m-2 p-2 w-93 bg-gray-100">
                    <p className="p-2 font-bold font-serif">{auth.currentUser.email}</p>
                </div>
                <div>
                    <p className="font-bold m-2 text-left">User Name:</p>
                    <p className="border-2 border-black rounded-2xl m-2 w-93 bg-gray-100 p-4 font-bold font-serif">{userName}</p>
                </div>
                <button className="rounded-2xl no-underline w-34 h-13 text-white my-bg-blue p-4 m-2 text-center mt-6 hover:opacity-75" onClick={async () => {await signOut(auth); navigate('/')}}>Sign Out</button>
                <button className="rounded-2xl no-underline w-38 h-13 text-white my-bg-blue p-4 m-2 text-center ml-18.5 mt-6 hover:opacity-75" onClick={() => {navigate('/userPage')}}>View User Page</button>
            </div>}
            <div className="h-2 bg-gray-100 mt-5 m-0 p-0 w-full">

            </div>

            <div className="grid grid-cols-4 gap-2 text-center text-xs mt-3 p-4">
                {menu.map((cmd, i) => {
                    return <div key={i}>
                                <div className="flex items-center justify-center h-15 w-15 flex-col bg-gray-100 rounded-2xl p-2 hover:bg-gray-200 cursor-pointer">
                                    {cmd.icon}
                                </div>
                                <div className="text-center h-15 w-15 mt-2 font-medium">
                                    {cmd.label}
                                </div>
                            </div>
            })}
            </div>

            <div className="">
                <a className="p-2 w-full h-15 leading-10.5 font-medium hover:bg-gray-200">{<MapPin className="inline w-9"/>} My Maps</a>
                <a className="p-2 w-full h-15 leading-10.5 font-medium hover:bg-gray-200">{<Bus className="inline w-9"/>} Tabs and My Transport</a>
                <a className="p-2 w-full h-15 leading-10.5 font-medium hover:bg-gray-200">{<Camera className="inline w-9"/>} Comments and Photos</a>
            </div>
        </div>
    );
}


export default Menu;