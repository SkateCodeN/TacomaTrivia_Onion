import tacomaPic from "../../../assets/MtRainer.jpg"
// src/components/HeroTacoma.tsx
import { Button, Container, Group, Overlay, Text, Title } from "@mantine/core";
import classes from "./HeroTacoma.module.css";

export default function Hero() {
  // Replace with your local asset path once you add the image to /public or /assets
  const bgUrl = "/assets/MtRainer_4x.jpg"; // e.g., public/MtRainer_4x.jpg

  return (
    <div className={classes.wrapper} aria-label="Tacoma waterfront with Mount Rainier at sunrise">
      <Overlay
        gradient="linear-gradient(180deg, rgba(0,0,0,0.55) 0%, rgba(0,0,0,0.35) 40%, rgba(0,0,0,0.75) 100%)"
        opacity={1}
        zIndex={0}
      />
      <div className={classes.bg} style={{ backgroundImage: `url(${tacomaPic})` }} />

      <Container size="lg" className={classes.inner}>
        <Title className={classes.title}>
          Tacoma Trivia
          <Text component="span" inherit className={classes.accent}>
            {" "}Play • Learn • Win
          </Text>
        </Title>

        <Text className={classes.description}>
          Weekly live games, local venues, real prizes. Build your team, climb the leaderboard,
          and show Tacoma what you know.
        </Text>

        <Group mt="md">
          <Button size="md" radius="xl" onClick={() => (window.location.href = "/venues")}>
            Find a Game
          </Button>
          <Button
            size="md"
            radius="xl"
            variant="outline"
            color="gray"
            onClick={() => (window.location.href = "/venues/apply")}
          >
            Add your venue
          </Button>
        </Group>
      </Container>
    </div>
  );
}
