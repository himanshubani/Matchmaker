const express = require('express');
const cors = require('cors');
const mongoose = require('mongoose');
require('dotenv').config();

const app = express();
const PORT = 5000;

app.use(cors());
app.use(express.json());

mongoose.connect(process.env.MONGODBURI)
.then(() => console.log('MongoDB Atlas connected'))
.catch(err => console.error('Connection error:', err));

const userSchema = new mongoose.Schema({
  name: String,
  age: Number,
  interests: [String],
  shortlist: [String],
});

const User = mongoose.model('User', userSchema);

app.post('/users', async (req, res) => {
  const { name, age, interests } = req.body;
  if (!name || !age || !interests) {
    return res.status(400).json({ error: 'Invalid input' });
  }
  await User.create({ name, age, interests, shortlist: [] });
  res.status(201).json({ message: 'User created' });
});

app.get('/matches/:username', async (req, res) => {
  const { username } = req.params;
  const user = await User.findOne({ name: new RegExp(`^${username}$`, 'i') });
  if (!user) return res.status(404).json({ error: 'User not found' });

  const allUsers = await User.find({ name: { $ne: user.name } });
  const matches = allUsers.filter(other =>
    other.interests.filter(i => user.interests.includes(i)).length >= 2
  );
  res.json(matches);
});

app.listen(PORT, () => console.log(`Server running on http://localhost:${PORT}`));
