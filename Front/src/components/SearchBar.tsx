import React from 'react';
import { useState } from 'react';
import { MapPin, Search } from 'lucide-react';

interface SearchBarProps {
    onSearch: (coords: [number, number]) => void;
}

const SearchBar: React.FC<SearchBarProps> = ({onSearch}) => {
    const [query, setQuery] = useState('');

    const handleSearch = async () => {
        const res = await fetch(`https://nominatim.openstreetmap.org/search?q=${query}&format=json&limit=10`);
        const data = await res.json();
        if (data.length > 0)
        {
            const {lat, lon} = data[0];
            onSearch([parseFloat(lat), parseFloat(lon)]);
        }
    }


    return (
        <div className="flex items-center gap-2 bg-gray-100 rounded-full px-4 py-2 shadow-sm">
            <MapPin className="w-5 h-5 text-red-500" />
            <input
            type="text"
            value={query}
            placeholder="Search..."
            onChange={(e) => {setQuery(e.target.value)}}
            className="bg-transparent flex-1 outline-none text-sm"
            />
            <Search onClick={handleSearch} className="w-7 h-7 text-gray-500 hover:bg-gray-200 cursor-pointer rounded-full p-1" />
        </div>
    );
}

export default SearchBar;