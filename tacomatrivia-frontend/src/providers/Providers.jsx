import { MantineProvider, useMantineColorScheme } from '@mantine/core';
import { Notifications } from '@mantine/notifications';
import { IconMoon, IconSun } from '@tabler/icons-react';
import { ActionIcon, Group } from '@mantine/core';

export default function Providers({ children }) {
  const { colorScheme, toggleColorScheme } = useMantineColorScheme();

  return (
    <MantineProvider defaultColorScheme={colorScheme}>
      <Notifications position="top-right" />
      {children}
    </MantineProvider>
  );
}

export function ColorSchemeToggle() {
  const { colorScheme, toggleColorScheme } = useMantineColorScheme();

  return (
    <ActionIcon
      onClick={toggleColorScheme}
      variant="default"
      size="xl"
      aria-label="Toggle color scheme"
    >
      {colorScheme === 'dark' ? (
        <IconSun size={18} />
      ) : (
        <IconMoon size={18} />
      )}
    </ActionIcon>
  );
}
