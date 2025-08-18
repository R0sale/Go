import { auth } from "../firebase";
import guestImage from "../assets/guest.png";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import type { MyFacility } from "../models/MyFacility";
import { config } from "../config";
import type { TokenResult } from "../models/TokenResult";



const UserPage: React.FC = () => {
    const navigate = useNavigate();
    const [tokenResult, setTokenResult] = useState<TokenResult>({
    userName: '',
    firstName: '',
    lastName: '',
    email: '',
    roles: [],
});
    const [loading, setLoading] = useState(false);
    const [facilities, setFacilities] = useState<MyFacility[]>([]);

    useEffect(() => {
        setLoading(true);

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
                    roles: [],
                });
            }

            setLoading(false);
        });

        return () => unsubscribe();
    }, []);

    const getFacilities = async () => {
        if (tokenResult.roles.map((role: string) => role == 'Admin'))
        {
            const token = await auth.currentUser.getIdToken();

            try {
                const result = await fetch(config.USERS_FACILITIES_URL, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });
            
                const response = await result.json();

                setFacilities(response);

            } catch (er: any) {
                if (er instanceof Error)
                {
                    alert(`Error: ${er.message}`);
                } else
                {
                    alert(`Some undefined error occured.`);
                }
            }
            
        }
    }

    if (loading) {
        return (<div className="flex h-screen w-screen">
            <span className="loader m-auto"></span>
        </div>);
    }

    const data = [
        {label: "Username", data: `${tokenResult.userName}`},
        {label: "Full name", data: `${tokenResult.firstName} ${tokenResult.lastName}`},
        {label: "Email", data: `${tokenResult.email}`},
        {label: "Roles", data: `${tokenResult.roles}`}
    ];

    return (
        <div className="h-screen flex pinned-left bg-gray-100 overflow-y-hidden">
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
                    <div className="ml-4 font-semibold w-full h-10 text-2xl flex justify-between">
                        <p>Account</p>
                        {tokenResult.roles.includes('Admin') ? <button className="w-48 h-15 text-center items-center text-xl mr-113 flex border-2 border-gray-300" onClick={() => {navigate('/userPage/facilityPage')}}>Create New Facility</button> : <div>No</div>}
                    </div>
                    
                </div>
                <div className="h-1 bg-gray-100 mt-5 m-0 p-0 w-full"></div>
                <div className="flex h-109">
                    <div className="w-2/3">
                        {data.map((row, i) => {
                            return (<div className="ml-4 mt-4 text-xl justify-between flex" key={i}>
                            <p className="p-2">{row.label}</p>
                            <p className="mr-10 w-140 p-2  rounded-2xl border-1 bg-gray-100">{row.data}</p>
                        </div>);
                        })}
                    </div>
                    <div className="w-1/3 m-2 h-100">
                        <div className="bg-gray-100 rounded-2xl border-2 border-black h-full">
                            <div className="flex justify-between">
                                <p className="m-2 mt-4 text-2xl w-1/3">Your facilities:</p>
                                <button className="w-35 h-12 text-center items-center text-l m-2 border-2 border-gray-300" onClick={getFacilities}>Get Facilities</button>
                            </div>
                            <p className="h-1 bg-white m-0 p-0 w-full"></p>
                            <div className="overflow-y-scroll h-80">
                                {facilities.map((facility, i) => {
                                    return (
                                        <div key={i} className="p-2 w-full h-15 leading-10.5 font-medium cursor-pointer hover:bg-gray-200" onClick={() => {navigate(`/userPage/facilityPage/${facility.id}`)}}>
                                            {facility.name}: {facility.email}
                                        </div>
                                    );
                                })}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default UserPage;