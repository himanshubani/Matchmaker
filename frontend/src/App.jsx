import { useEffect, useState } from 'react';
import axios from 'axios';
import { UserIcon, HeartIcon } from '@heroicons/react/24/solid';
import { motion } from 'framer-motion';

function App() {
  const [name, setName] = useState('');
  const [age, setAge] = useState('');
  const [interests, setInterests] = useState('');
  const [username, setUsername] = useState('');
  const [matches, setMatches] = useState([]);
  const [shortlisted, setShortlisted] = useState([]);
  const [hasSearched, setHasSearched] = useState(false);
  const [darkMode, setDarkMode] = useState(false);
  const [ageFilter, setAgeFilter] = useState('');
  const [interestFilter, setInterestFilter] = useState('');

  // Load shortlist from localStorage on mount
  useEffect(() => {
    const stored = JSON.parse(localStorage.getItem('shortlisted')) || [];
    setShortlisted(stored);
  }, []);

  // Save to localStorage when shortlist changes
  useEffect(() => {
    localStorage.setItem('shortlisted', JSON.stringify(shortlisted));
  }, [shortlisted]);

  const submitProfile = async () => {
    await axios.post('https://matchmaker-sgg2.onrender.com/users', {
      name,
      age: parseInt(age),
      interests: interests.split(',').map(i => i.trim())
    });
    alert('Profile submitted');
  };

  const findMatches = async () => {
    const res = await axios.get(`https://matchmaker-sgg2.onrender.com/matches/${username}`);
    setMatches(res.data);
    setHasSearched(true);
  };

  const handleShortlist = (matchName) => {
    if (!shortlisted.includes(matchName)) {
      setShortlisted(prev => [...prev, matchName]);
    }
  };

  const avatarUrl = (name) =>
    `https://avatars.dicebear.com/api/initials/${encodeURIComponent(name)}.svg`;

  const filteredMatches = matches.filter(m => {
    const ageMatch = ageFilter ? m.age == ageFilter : true;
    const interestMatch = interestFilter
      ? m.interests.some(i =>
          i.toLowerCase().includes(interestFilter.toLowerCase())
        )
      : true;
    return ageMatch && interestMatch;
  });

  return (
    <div className={darkMode ? 'dark' : ''}>
      <div className="min-h-screen bg-gray-100 dark:bg-gray-900 text-gray-900 dark:text-gray-100 py-10 px-4">
        <div className="max-w-2xl mx-auto bg-white dark:bg-gray-800 p-8 rounded-xl shadow-lg space-y-8 relative">

          <h1 className="text-3xl font-bold text-center text-indigo-600 dark:text-indigo-400">
            Matchmaker 
          </h1>

          {/* User Profile Form */}
          <div className="space-y-4">
            <h2 className="text-xl font-semibold flex items-center gap-2">
              <UserIcon className="w-5 h-5 text-gray-500" />
              Create Your Profile
            </h2>
            <input placeholder="Name" value={name} onChange={e => setName(e.target.value)} className="border dark:border-gray-600 bg-white dark:bg-gray-700 px-4 py-2 w-full rounded" />
            <input placeholder="Age" value={age} onChange={e => setAge(e.target.value)} className="border dark:border-gray-600 bg-white dark:bg-gray-700 px-4 py-2 w-full rounded" />
            <input placeholder="Interests (comma separated)" value={interests} onChange={e => setInterests(e.target.value)} className="border dark:border-gray-600 bg-white dark:bg-gray-700 px-4 py-2 w-full rounded" />
            <button onClick={submitProfile} className="w-full bg-indigo-600 text-white py-2 rounded hover:bg-indigo-700 transition">
              Submit Profile
            </button>
          </div>

          {/* Find Matches */}
          <div className="space-y-4">
            <h2 className="text-xl font-semibold">Find Matches</h2>
            <input placeholder="Enter your username" value={username} onChange={e => setUsername(e.target.value)} className="border dark:border-gray-600 bg-white dark:bg-gray-700 px-4 py-2 w-full rounded" />
            <button onClick={findMatches} className="w-full bg-green-500 text-white py-2 rounded hover:bg-green-600 transition">
              Find Matches
            </button>

            <div className="flex gap-2 mt-4">
              <input placeholder="Filter by age" value={ageFilter} onChange={e => setAgeFilter(e.target.value)} className="border dark:border-gray-600 bg-white dark:bg-gray-700 px-4 py-2 rounded w-1/2" />
              <input placeholder="Filter by interest" value={interestFilter} onChange={e => setInterestFilter(e.target.value)} className="border dark:border-gray-600 bg-white dark:bg-gray-700 px-4 py-2 rounded w-1/2" />
            </div>
          </div>

          {/* Matches Section */}
          <div>
            <h3 className="text-lg font-semibold mb-2">Matches:</h3>
            {hasSearched && filteredMatches.length === 0 ? (
              <p className="text-gray-500 italic">No matches found for this username.</p>
            ) : (
              <div className="space-y-4">
                {filteredMatches.map(m => (
                  <motion.div
                    key={m._id}
                    className="flex items-center gap-4 border dark:border-gray-600 p-4 rounded-lg bg-gray-50 dark:bg-gray-700 shadow-sm"
                    initial={{ opacity: 0, y: 20 }}
                    animate={{ opacity: 1, y: 0 }}
                    transition={{ duration: 0.3 }}
                  >
                    <img src={avatarUrl(m.name)} alt="avatar" className="w-12 h-12 rounded-full" />
                    <div className="flex-grow">
                      <p className="font-bold">{m.name} <span className="text-sm text-gray-500">({m.age})</span></p>
                      <p className="text-sm text-gray-300">Interests: {m.interests.join(', ')}</p>
                    </div>
                    <button
                      onClick={() => handleShortlist(m.name)}
                      className="flex items-center gap-1 text-blue-600 hover:text-blue-800 text-sm"
                    >
                      <HeartIcon className="w-5 h-5" />
                      Shortlist
                    </button>
                  </motion.div>
                ))}
              </div>
            )}
          </div>

          {/* Shortlisted */}
          {shortlisted.length > 0 && (
            <div>
              <h3 className="text-lg font-semibold">Shortlisted Matches:</h3>
              <ul className="list-disc list-inside mt-2 space-y-1">
                {shortlisted.map(name => (
                  <li key={name}>{name}</li>
                ))}
              </ul>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}

export default App;
