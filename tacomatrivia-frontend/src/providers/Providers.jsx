import { MantineProvider } from '@mantine/core';
import { Notifications } from '@mantine/notifications';

export default function Providers({ children }) {
  return (
    <MantineProvider defaultColorScheme="light">
      <Notifications position="top-right" />
      {children}
    </MantineProvider>
  );
}
