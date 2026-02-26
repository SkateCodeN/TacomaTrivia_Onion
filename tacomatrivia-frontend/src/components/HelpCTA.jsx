// src/components/WhyTacomaTrivia.jsx
import { Container, Title, Text, SimpleGrid, Stack } from '@mantine/core';
import styles from '../styles/HelpCTA.module.css';

export default function HelpCTA() {
  return (
    <div className={styles.section}>
      <Container size="lg">
        <Title order={2} ta="center" mb="xl">
          Wanna help TacomaTrivia?
        </Title>

        <SimpleGrid cols={{ base: 1, md: 3 }} spacing="xl">
          <Stack>
            <Title order={4}>Maintain the dataset </Title>
            <Text>
              It takes a village of dedicated peeps to help maintain the integrity of the dataset.
            </Text>
          </Stack>

          <Stack>
            <Title order={4}>Give me your ideas!</Title>
            <Text>
              This app started for a love for data and an adhd hyperfixation, send me your 
              ideas to keep this app alive.
            </Text>
          </Stack>

          <Stack>
            <Title order={4}>Help Spread the love</Title>
            <Text>
              If you like the content, let other people know! I want to test how scalable
              this baby can be.
            </Text>
          </Stack>
        </SimpleGrid>
      </Container>
    </div>
  );
}