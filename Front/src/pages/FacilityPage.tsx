import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { auth } from "../firebase";
import type { MyFacility } from "../models/MyFacility";
import facilityImage from "../assets/facility.png";
import FacilityMap from "./components/FacilityMap";
import type { DayOfWeek } from "../models/DayOfWeek";


const FacilityPage: React.FC = () => {
    const { facilityId } = useParams();
    const [facility, setFacility] = useState<MyFacility | null>(null);
    const [loading, setLoading] = useState(false);
    const [time, setTime] = useState(new Date());
    const navigate = useNavigate();

    useEffect(() => {
        setLoading(true);

        const unsubscribe = auth.onAuthStateChanged(async (user) => {
            if (user)
            {
                

                const token = await user.getIdToken();

                const result = await fetch(`http://localhost:5000/api/facility/${facilityId}`, {
                    method: 'GET',
                    headers: {
                        'Authorization': `Bearer ${token}`
                    }
                });

                const facility = await result.json();
                setFacility(facility);
            }

            setLoading(false);
        });

        return () => unsubscribe();

    }, [facilityId]);

    useEffect(() => {
        const timerId = setInterval(() => {
            setTime(new Date());
        }, 60000);

        return () => clearInterval(timerId);
    });

    const parseTimeToMinutes= (timeString: string) => {
        const [hours, minutes] = timeString.split(':').map(Number);
        return hours * 60 + minutes;
    }

    const isOpen = () => {
        if (!facility) 
            return 'Closed';

        const currentTimeInMinutes = time.getHours() * 60 + time.getMinutes();
        const dayMapping: DayOfWeek[] = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
        const dayOfWeek: DayOfWeek = dayMapping[time.getDay()];

        const timeStr = facility.schedule[dayOfWeek];

        if (timeStr.toLowerCase() == 'closed' || timeStr == null)
            return 'Close';

        const [openTime, closeTime] = timeStr.split('-');

        if (!openTime || !closeTime)
            return 'Unknown';

        const openTimeInMinutes = parseTimeToMinutes(openTime);
        const closeTimeInMinutes = parseTimeToMinutes(closeTime);

        let isOpen: boolean;

        if (closeTimeInMinutes > openTimeInMinutes)
            isOpen = currentTimeInMinutes < closeTimeInMinutes && currentTimeInMinutes > openTimeInMinutes;
        else
            isOpen = currentTimeInMinutes < closeTimeInMinutes || currentTimeInMinutes > openTimeInMinutes;

        return isOpen ? 'Open' : 'Close';
    }

    if (loading) {
        return (<div className="flex h-screen w-screen">
            <span className="loader m-auto"></span>
        </div>);
    }

    if (!facility) {
        return <div>No Information</div>
    }

    const facilityData = [
        {label: 'Name', data: `${facility?.name}`},
        {label: 'Email', data: `${facility?.email}`},
        {label: 'Phone', data: `${facility?.phoneNumber}`},
        {label: 'Website', data: `${facility?.websiteURL}`},
    ];

    const schedule = [
        {day: 'Monday', data: `${facility.schedule.Monday}`},
        {day: 'Tuesday', data: `${facility.schedule.Tuesday}`},
        {day: 'Wednesday', data: `${facility.schedule.Wednesday}`},
        {day: 'Thursday', data: `${facility.schedule.Thursday}`},
        {day: 'Friday', data: `${facility.schedule.Friday}`},
        {day: 'Saturday', data: `${facility.schedule.Saturday}`},
        {day: 'Sunday', data: `${facility.schedule.Sunday}`},
    ];

    return (
    <div className="bg-gray-100 h-screen flex">
        <div className="bg-white m-2 w-screen flex">
            <div className="block">
                <img src={facilityImage} alt="Facility icon" className="bg-gray-100 rounded-full w-90 h-90 m-20 mb-0"></img>
                <div className="border-1 h-96 rounded-2xl m-20 mt-5 bg-gray-100">
                    <p className="m-2 text-3xl">Schedule: </p>
                    {schedule.map((day, i) => {
                        return (<div key={i} className="text-2xl m-2">
                            {day.day}: {day.data}
                        </div>)
                    })}
                    <p className="m-2 text-2xl">Is open: {isOpen()}</p>
                </div>
            </div>
            <div className="block h-full">
                {facilityData.map((facility, i) => {
                                return (<div className="ml-4 mt-4 text-xl justify-between flex" key={i}>
                                <p className="p-2">{facility.label}</p>
                                <p className="ml-20 w-140 p-2  rounded-2xl border-1 bg-gray-100">{facility.data}</p>
                            </div>);
                            })}
                <div className="rounded-2xl border-1 block bg-gray-100 h-129 w-full mt-20 text-xl">
                    <p className="m-2">{facility?.description}</p>
                </div>
            </div>
            <div className="ml-10 block">
                <FacilityMap coords={[facility.coordinates.latitude, facility.coordinates.longitude]}/>
                <button className="w-48 h-15 text-center absolete items-center text-xl  flex border-2 mt-77 ml-99 border-gray-300" onClick={() => {navigate('/')}}>To Main Page</button>
            </div>
            
        </div>
    </div>);
}

export default FacilityPage;