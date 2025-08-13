import { auth } from "../firebase";
import guestImage from "../assets/guest.png";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";



const UserPage: React.FC = () => {
    const navigate = useNavigate();
    const [tokenResult, setTokenResult] = useState({
    userName: '',
    firstName: '',
    lastName: '',
    email: '',
    roles: '',
});
    useEffect(() => {
        const unsubscribe = auth.onAuthStateChanged(async (user) => {
            if (user) {
                const token = await user.getIdTokenResult();
                console.log(token.claims.roles);
                setTokenResult(token.claims);
            } else {
                setTokenResult({
                    userName: '',
                    firstName: '',
                    lastName: '',
                    email: '',
                    roles: '',
                });
            }
        });

        

        return () => unsubscribe();
    }, []);

        const data = [
        {label: "Username", data: `${tokenResult.userName}`},
        {label: "Full name", data: `${tokenResult.firstName} ${tokenResult.lastName}`},
        {label: "Email", data: `${tokenResult.email}`},
        {label: "Roles", data: `${tokenResult.roles}`}
    ];

    return (
        <div className="h-screen flex pinned-left bg-gray-100">
            <div className="mt-4 mb-4 ml-6 mr-6 w-screen bg-white ">
                <div className="h-1/2 bg-white">
                    <div className="p-20 flex">
                        <img className="rounded-full w-60 h-60 block" src={guestImage} ></img>
                        <div className="ml-100">
                            <p className="text-6xl font-bold">{tokenResult.firstName} {tokenResult.lastName}</p>
                            <p className="text-2xl text-blue-600 underline mt-4 decoration-2">{tokenResult.email}</p>
                        </div>
                        <button className="w-48 h-15 text-center items-center text-xl ml-150 flex border-2 border-gray-300" onClick={() => {navigate('/')}}>To Main Page</button>
                    </div>
                    <div className="ml-4 font-semibold w-full h-10 text-2xl">
                        Account
                    </div>
                </div>
                <div className="h-1 bg-gray-100 mt-5 m-0 p-0 w-full"></div>
                <div className="w-2/3">
                    {data.map((row, i) => {
                        return (<div className="ml-4 mt-4 text-xl justify-between flex" key={i}>
                        <p className="p-2">{row.label}</p>
                        <p className="mr-10 w-140 p-2  rounded-2xl border-1 bg-gray-100">{row.data}</p>
                    </div>);
                    })}
                </div>
            </div>
        </div>
    );
}

export default UserPage;