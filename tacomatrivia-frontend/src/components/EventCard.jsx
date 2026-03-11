// src/components/EventCard.jsx
import { Card, Text, Badge, Group, Button } from '@mantine/core';
import styles from '../styles/EventCard.module.css';
import DayConverter from '@features/venues/helper functions/DayConverter';
import TimeConverter from '@features/venues/helper functions/TimeConverter';
import CompareDateTime from '@features/venues/helper functions/CompareDateTime';
import { useEffect, useState } from 'react';
import axios from 'axios';
export default function EventCard({ venueData, day }) {

  const [urlData, setUrlData] = useState([]);

  const formattedTime = TimeConverter(venueData.triviaStart)
  const currentDay = DayConverter(day);

  //console.log("Venue: ", venueData.name);
  //const onTime = CompareDateTime(venueData.triviaStart);
  //console.log("Is the venue still hosting the trivia event? : ", onTime)

  const badgeColor = () => {
    if (CompareDateTime(venueData.triviaStart)) {
      return 'badgeColor.2'
    }
    else {
      return 'badgeColor.0'
    }
  }

  const eventState = () => {
    if (CompareDateTime(venueData.triviaStart)) {
      return 'Upcoming'
    }
    else {
      return 'Event over'
    }
  }

  const handleOnClick = async (address) => {
    // const response = await axios.get('https://proxy.corsfix.com/?' + "https://nominatim.openstreetmap.org/search?format=jsonv2&q=158+100th+St+S+Tacoma+WA+98444", {
    //   headers: {
    //     'Content-Type': 'application/json',
    //     'Authorization': null,
    //     "User-Agent": navigator.userAgent
    //   }
    // });

    //const data = response.data;

    //setUrlData(data);

    //if (data && data.length > 0) {
      // const lat = data[0].lat;
      // const lon = data[0].lon;

      // Note: Fixed your template literal for the URL
      //const reStr = `https://www.google.com/maps?q=${lat},${lon}`;
      const reStr2 = `https://www.google.com/maps?q=${venueData.name.replace(" ",'+')}`
      //const googleMapsRe = `https://www.google.com/maps?q=${venueData.address}`
      
      //console.log("Redirecting to:", reStr);
      console.log("Redirecting to:", reStr2);
      //console.log("Redirecting to:", googleMapsRe);
      // window.location.href = reStr;
    //} else {
      //console.error("No results found for this address.");
    //}
    window.open(reStr2);
  }


  // Badges for in 15 min ... red
  // 30 min ... yellow
  // 1 hour ... red
  // If its past the hour, delete from list
  return (
    <Card shadow="md" radius="lg" padding="lg" className={styles.card}>
      <Group justify="space-between" mb="sm" wrap="nowrap" align="flex-start" h={40}>
        <Text fw={600} style={{wordBreak:"break-word", flex: 1}}>{venueData.name}</Text>
        {/* variant="light" */}
        <Badge color={badgeColor()} style={{ flexShrink: 0 }} >
          {eventState()}
        </Badge>
      </Group>

      <Text size="sm" c="dimmed">
        {currentDay} • {formattedTime}
      </Text>

      <Text size="sm" mt="sm" >
        {venueData.address}
      </Text>

      <Button fullWidth mt="md" radius="xl" onClick={() => handleOnClick(venueData.address)}>
        Location
      </Button>
    </Card>
  );
}