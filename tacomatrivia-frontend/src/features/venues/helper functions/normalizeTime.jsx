// Returns "HH:mm" (24h) or null if invalid
export function normalizeTime(input){
  if (!input) return null;

  const s = String(input).trim().toLowerCase();

  // Matches: H, HH, H:MM, HH:MM, with optional am/pm (with or without space)
  const m = s.match(/^(\d{1,2})(?::(\d{1,2}))?\s*(am|pm)?$/i);
  if (!m) return null;

  let hour = Number(m[1]);
  let minute = m[2] != null ? Number(m[2]) : 0;
  const meridian = m[3]?.toLowerCase();

  // Validate minute
  if (!Number.isInteger(minute) || minute < 0 || minute > 59) return null;

  if (meridian) {
    // 12-hour clock rules
    if (hour < 1 || hour > 12) return null;
    if (meridian === 'am') {
      hour = hour % 12; // 12am -> 0
    } else {
      hour = (hour % 12) + 12; // 12pm -> 12, 1pm -> 13, etc.
    }
  } 
  else {
    // 24-hour clock rules
    if (!Number.isInteger(hour) || hour < 0 || hour > 23) return null;
  }

  const hh = hour.toString().padStart(2, '0');
  const mm = minute.toString().padStart(2, '0');
  return `${hh}:${mm}:00`;
}
