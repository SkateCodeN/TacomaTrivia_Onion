// src/components/FeaturedEvents.jsx
import { Title, SimpleGrid } from '@mantine/core';
import { useEffect,useState } from 'react';
import EventCard from './EventCard';
import axios from 'axios';
import DayConverter from '@features/venues/helper functions/DayConverter';
export default function FeaturedEvents() {
  const [venues, setVenues] = useState([]);
  const [day, setDay] = useState("");
  
  //Get the date for today
  const getTodayDate = () =>{
    const currentDate = new Date(); 
    
    const dayOfWeekNumber = currentDate.getDay();
    return dayOfWeekNumber;
  }

  

  const  getCurrentTriviabyDay = async () =>
  {
    const currentDay = getTodayDate();
    setDay(currentDay);
    const response = await axios.get(`api/venues/${currentDay}`);
    setVenues(response.data);
  }
  useEffect(() =>{
    getCurrentTriviabyDay();
  },[])
  
  return (
    <>
      <Title order={2} mb="lg">
       {DayConverter(day)} Trivia
      </Title>

      <SimpleGrid cols={{ base: 1, sm: 2, md: 3 }} spacing="lg">
       {
        venues.map( venue => (
          <EventCard key={venue.id} venueData={venue} day={day} />
        ))
       }
      </SimpleGrid>
    </>
  );
}