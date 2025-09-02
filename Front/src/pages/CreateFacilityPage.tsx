import React, { useEffect, useState} from "react";
import bgImage from '../assets/worldmap.jpg';
import { MapContainer, Marker, TileLayer, Tooltip} from "react-leaflet";
import getPosition from "../GetPosition";
import type { LatLng, LatLngExpression } from "leaflet";
import { auth } from "../firebase";
import { config } from "../config";
import MapClickHandler from "./components/MapClickHandler";
import { useNavigate } from "react-router-dom";
import * as z from "zod";

const CreateFacilityPage: React.FC = () => {
    const initialSchedule = [
        {id: 1, day: 'Monday', time: {
            start: { h: '09', m: '00'},
            end: { h: '18', m: '00'}
        }},
        {id: 2, day: 'Tuesday', time: {
            start: { h: '09', m: '00'},
            end: { h: '18', m: '00'}
        }},
        {id: 3, day: 'Wednesday', time: {
            start: { h: '09', m: '00'},
            end: { h: '18', m: '00'}
        }},
        {id: 4, day: 'Thursday', time: {
            start: { h: '09', m: '00'},
            end: { h: '18', m: '00'}
        }},
        {id: 5, day: 'Friday', time: {
            start: { h: '09', m: '00'},
            end: { h: '18', m: '00'}
        }},
        {id: 6, day: 'Saturday', time: {
            start: { h: '09', m: '00'},
            end: { h: '18', m: '00'}
        }},
        {id: 7, day: 'Sunday', time: {
            start: { h: '09', m: '00'},
            end: { h: '18', m: '00'}
        }}
    ];

    const [name, setName] = useState('');
    const [email, setEmail] = useState('');
    const [phone, setPhone] = useState('');
    const [website, setWebsite] = useState('');
    const [description, setDescription] = useState('');
    const [schedule, setSchedule] = useState(initialSchedule);
    const [center, setCenter] = useState<LatLngExpression>();
    const [loading, setLoading] = useState(false);
    const [coords, setCoords] = useState<LatLng>();
    const [position, setPosition] = useState<LatLngExpression>();
    const navigate = useNavigate();

    useEffect(() => {
        setLoading(true);
        const getPos = async () => {
            const pos = await getPosition();
            setCenter([pos.coords.latitude, pos.coords.longitude]);
        }

        getPos();

        setLoading(false);
    }, [center]);

    const handleTimeChange = (dayId: number, event: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = event.target;

        const [ timeType, unit ] = name.split('-');

        if ((timeType === 'start' || timeType === 'end') && (unit === 'h' || unit === 'm')) {
            setSchedule(currentSchedule => 
            currentSchedule.map((day) => {
                if (day.id == dayId) {
                    return {
                        ...day,
                        time: {
                            ...day.time,
                            [timeType]: {
                                ...day.time[timeType],
                                [unit]: value
                            }
                        }
                    }
                } else {
                    return day;
                }
            })
        );
        }
    }

    const eventHandler = {
        click: () => {
            console.log('ya');
        }
    }

    type ScheduleObj = Record<string, string>;

    const validate = (facility: object) => {
        const Facility = z.object({
            name: z.string().min(4).max(20),
            email: z.email(),
            phoneNumber: z.string().regex(/^[+][0-9]{11,15}$/, 'Phone number must be valid'),
            websiteURL: z.string().min(10).regex(/^(http:\/\/|https:\/\/)[A-Za-z0-9?=_\-/.]*$/),
            description: z.string().min(20),
            schedule: z.object({
                Monday: z.string().regex(/^\d{2}:\d{2}-\d{2}:\d{2}$/),
                Tuesday: z.string().regex(/^\d{2}:\d{2}-\d{2}:\d{2}$/),
                Wednesday: z.string().regex(/^\d{2}:\d{2}-\d{2}:\d{2}$/),
                Thursday: z.string().regex(/^\d{2}:\d{2}-\d{2}:\d{2}$/),
                Friday: z.string().regex(/^\d{2}:\d{2}-\d{2}:\d{2}$/),
                Saturday: z.string().regex(/^\d{2}:\d{2}-\d{2}:\d{2}$/),
                Sunday: z.string().regex(/^\d{2}:\d{2}-\d{2}:\d{2}$/)
            }),
            coordinates: z.object({
                latitude: z.number().gte(-90).lte(90),
                longitude: z.number().gte(-180).lte(180)
            })
        });

        Facility.parse(facility);
    }

    const createFacility = async () => {
        try {
            console.log(schedule.reduce((accumulator, day) => {
                            const formattedStr = `${day.time.start.h}:${day.time.start.m}-${day.time.end.h}:${day.time.end.m}`;

                            accumulator[day.day] = formattedStr;

                            return accumulator;
                        }, {} as ScheduleObj));

            validate({
                    name: `${name}`,
                    email: `${email}`,
                    phoneNumber: `${phone}`,
                    websiteURL: `${website}`,
                    description: `${description}`,
                    schedule: 
                        schedule.reduce((accumulator, day) => {
                            const formattedStr = `${day.time.start.h}:${day.time.start.m}-${day.time.end.h}:${day.time.end.m}`;

                            accumulator[day.day] = formattedStr;

                            return accumulator;
                        }, {} as ScheduleObj),
                    coordinates: {
                        latitude: coords?.lat ?? 0,
                        longitude: coords?.lng ?? 0
                    }
                });

            const token = await auth.currentUser?.getIdToken(true);
            const result = await fetch(config.CREATE_FACILITY_URL, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    name: `${name}`,
                    email: `${email}`,
                    phoneNumber: `${phone}`,
                    websiteURL: `${website}`,
                    description: `${description}`,
                    schedule: 
                        schedule.reduce((accumulator, day) => {
                            const formattedStr = `${day.time.start.h}:${day.time.start.m}-${day.time.end.h}:${day.time.end.m}`;

                            accumulator[day.day] = formattedStr;

                            return accumulator;
                        }, {} as ScheduleObj),
                    coordinates: {
                        latitude: coords?.lat,
                        longitude: coords?.lng
                    }
                })
            });

            console.log(JSON.stringify({
                    name: `${name}`,
                    email: `${email}`,
                    phone: `${phone}`,
                    website: `${website}`,
                    description: `${description}`,
                    schedule: 
                        schedule.reduce((accumulator, day) => {
                            const formattedStr = `${day.time.start.h}:${day.time.start.m}-${day.time.end.h}:${day.time.end.m}`;

                            accumulator[day.day] = formattedStr;

                            return accumulator;
                        }, {} as ScheduleObj),
                    coordinates: {
                        latitude: coords?.lat,
                        longitude: coords?.lng
                    }
                }));

            if (result.ok) {
                alert('Good job! Now you have new facility');
                navigate('/userPage');
            } else {
                alert(result.statusText);
            }
        } catch (error) {
            if (error instanceof Error) {
                alert(`Grustics ${error.message}`);
            } else {
                alert('Some troubles.');
            }
                
        }
    }

    if (loading) {
        return (<div className="flex h-full w-full">
            <span className="loader m-auto"></span>
        </div>);
    }

    return (
        <div style={{backgroundImage: `url(${bgImage})`, backgroundRepeat: 'space repeat'}} className="w-screen h-screen overflow-x-hidden">
            <div className="w-1/3 bg-white m-auto">
                <div>
                    <label className="block m-2 font-semibold">Name</label>
                    <input placeholder="Enter name of your facility" value={name} className="w-full border-2 p-1 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {setName(e.target.value)}}></input>

                    <label className="block m-2 font-semibold">Email</label>
                    <input placeholder="Enter email of your facility" className="w-full p-1 border-2 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {setEmail(e.target.value)}}></input>

                    <label className="block m-2 font-semibold">Phone</label>
                    <input placeholder="Enter phone number of your facility" className="w-full p-1 border-2 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {setPhone(e.target.value)}}></input>

                    <label className="block m-2 font-semibold">Website</label>
                    <input placeholder="Enter URL of your website" className="w-full p-1 border-2 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {setWebsite(e.target.value)}}></input>

                    <label className="block m-2 font-semibold">Description</label>
                    <input placeholder="Enter your description" className="w-full p-1 border-2 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {setDescription(e.target.value)}}></input>

                    <label className="block m-2 font-semibold">Schedule</label>
                    {schedule.map(day => {
                        return <div className="flex h-15 leading-13 justify-between m-2">
                            <p className="w-30 font-serif font-semibold">{day.day}:</p>
                            <p className="">From</p>
                            <input placeholder="hours" name="start-h" value={day.time.start.h} className="w-17 text-center p-1 border-2 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {handleTimeChange(day.id, e)}}></input>
                            <input placeholder="minutes" name="start-m" value={day.time.start.m} className="w-17 text-center p-1 border-2 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {handleTimeChange(day.id, e)}}></input>
                            <p>To</p>
                            <input placeholder="hours" name="end-h" value={day.time.end.h} className="w-17 text-center p-1 border-2 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {handleTimeChange(day.id, e)}}></input>
                            <input placeholder="minutes" name="end-m" value={day.time.end.m} className="w-17 text-center p-1 border-2 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {handleTimeChange(day.id, e)}}></input>
                        </div>
                    })}
                </div>
                {center ? <div>
                    <MapContainer className="h-110 w-full m-2" center={center} zoom={16} scrollWheelZoom={true}>
                        <TileLayer 
                        eventHandlers={eventHandler}
                        attribution='&copy; OpenStreetMap contributors'
                        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                        />
                        <MapClickHandler onClick={(coords: LatLng) => {setPosition(coords); setCoords(coords)}} />
                        {position ? (<>
                         <Marker position={position}>
                            <Tooltip permanent={true}>
                                This is your facility's location
                            </Tooltip>
                         </Marker>
                         </>) : null}
                    </MapContainer>
                </div> : <div>Bez coordinat segodnya</div>}
                <button className="w-30 m-4 h-15 text-xl text-center border-2 border-gray-300" onClick={createFacility}>Submit</button>
            </div>
        </div>);
}

export default CreateFacilityPage;