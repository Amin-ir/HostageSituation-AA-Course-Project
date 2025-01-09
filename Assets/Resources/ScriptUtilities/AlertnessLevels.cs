public enum AlertnessLevel
{
	Thinking = -1, // is thinking (used when transiting between alertness levels)
	NonAlerted = 0, // Has not seen the player yet
	Suspicious, // Signs of player or manipulation e.g. a sound
	Alerted, // Is seeing the player
	AlertedSearching // Seen player earlier, searching (sweep) the area 
}