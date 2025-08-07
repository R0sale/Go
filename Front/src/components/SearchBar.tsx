import React, { useEffect, useRef } from 'react';
import { useState } from 'react';
import { MapPin, Search } from 'lucide-react';

interface SearchBarProps {
    onSearch: (coords: [number, number]) => void;
}

const SearchBar: React.FC<SearchBarProps> = ({onSearch}) => {
    const [query, setQuery] = useState('');
    const [isSearching, setIsSearching] = useState(false);
    const [places, setPlaces] = useState<{ latitude: number, longitude: number, name: string }[]>([]);

    const timeoutRef = useRef<number | null>(null);
    const blurTimeoutRef = useRef<number | null>(null);

    useEffect(() => {
        if (!query) {
            setPlaces([]);
            return;
        }

        if (timeoutRef.current) {
            clearTimeout(timeoutRef.current);
        }

        timeoutRef.current = window.setTimeout(() => {
            fetch(`https://nominatim.openstreetmap.org/search?q=${query}&format=json&limit=10`)
                .then(res => res.json())
                .then(data => {
                    const result = data.map((item: any) => ({
                        latitude: parseFloat(item.lat),
                        longitude: parseFloat(item.lon),
                        name: item.display_name
                    }));
                    setPlaces(result);
                })
                .catch(() => {
                    setPlaces([]);
                });
        }, 500);

        return () => {
            if (timeoutRef.current)
                clearTimeout(timeoutRef.current);
        }

    }, [query]);

    const handleSearch = () => {
        if (query.trim()) {
            fetch(`https://nominatim.openstreetmap.org/search?q=${query}&format=json&limit=10`)
                .then(res => res.json())
                .then(data => {
                    const result = data.map((item: any) => ({
                        latitude: parseFloat(item.lat),
                        longitude: parseFloat(item.lon)
                    }));
                    setPlaces(result);
                    setIsSearching(true);
                });
        }
    }


    return (
        <>
            <div className="flex items-center gap-2 bg-gray-100 rounded-full px-4 py-2 shadow-sm">
                <MapPin className="w-5 h-5 text-red-500" />
                <input
                type="text"
                value={query}
                placeholder="Search..."
                onChange={(e) => {
                    setQuery(e.target.value);
                }}
                onFocus={() => {
                    if (blurTimeoutRef.current)
                        clearTimeout(blurTimeoutRef.current);
                    setIsSearching(true)
                }}
                onBlur={() => {
                    blurTimeoutRef.current = window.setTimeout(() => {
                        setIsSearching(false);
                    }, 200);
                }}
                className="bg-transparent flex-1 outline-none text-sm"
                />
                <Search onClick={handleSearch} className="w-7 h-7 text-gray-500 hover:bg-gray-200 cursor-pointer rounded-full p-1" />
            </div>
            { isSearching && places.length > 0 &&
            <div className="bg-white shadow-md rounded p-2 mt-2 max-h-60 overflow-auto">
                {places.map((place, i) => {
                    return <div key={i} onMouseDown={() => {
                        onSearch([place.latitude, place.longitude]);
                        setIsSearching(false);
                        setQuery('');
                        setPlaces([]);
                    }}
                    className='cursor-pointer hover:bg-gray-100 px-2 py-1 rounded'
                    >
                        {place.name}
                    </div>
                })}
            </div>
            }
        </>
    );
}

export default SearchBar;