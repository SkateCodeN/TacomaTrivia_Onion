import { useState, useEffect } from 'react';
import { Combobox, TextInput, useCombobox, Group, Avatar, Text } from '@mantine/core';

export default function UserSelectWithCreate({ value, onChange, label = "Team Member", placeholder = "Search users...", onUserCreated }) {
  // Simulated authenticated users
  const [users, setUsers] = useState([
    { value: '1', label: 'Alice Johnson', email: 'alice@example.com' },
    { value: '2', label: 'Bob Smith', email: 'bob@example.com' },
    { value: '3', label: 'Carol Williams', email: 'carol@example.com' },
    { value: '4', label: 'David Brown', email: 'david@example.com' },
  ]);

  const combobox = useCombobox({
    onDropdownClose: () => combobox.resetSelectedOption(),
    onDropdownOpen: () => {
      const selected = users.find(u => u.value === value);
      if (selected) {
        setSearch(selected.label);
      }
    }
  });

  const [search, setSearch] = useState('');

  // Sync search field with selected value
  useEffect(() => {
    const selected = users.find(u => u.value === value);
    if (selected) {
      setSearch(selected.label);
    } else if (value === null) {
      setSearch('');
    }
  }, [value, users]);

  const exactOptionMatch = users.some((item) => item.label === search);
  
  const filteredOptions = users.filter((item) =>
    item.label.toLowerCase().includes(search.toLowerCase().trim())
  );

  const options = filteredOptions.map((item) => (
    <Combobox.Option value={item.value} key={item.value}>
      <Group gap="sm">
        <Avatar color="blue" size="sm" radius="xl">
          {item.label.charAt(0)}
        </Avatar>
        <div>
          <Text size="sm">{item.label}</Text>
          <Text size="xs" c="dimmed">{item.email}</Text>
        </div>
      </Group>
    </Combobox.Option>
  ));

  const handleCreate = () => {
    const newMember = {
      value: `new-${Date.now()}`,
      label: search,
      email: '',
      isNew: true,
    };
    
    setUsers((current) => [...current, newMember]);
    onChange?.(newMember.value);
    onUserCreated?.(newMember);
    setSearch(newMember.label);
    combobox.closeDropdown();
  };

  return (
    <Combobox
      store={combobox}
      withinPortal={true}
      onOptionSubmit={(val) => {
        if (val === '$create') {
          handleCreate();
        } else {
          onChange?.(val);
          const selected = users.find(u => u.value === val);
          if (selected) {
            setSearch(selected.label);
          }
          combobox.closeDropdown();
        }
      }}
    >
      <Combobox.Target>
        <TextInput
          label={label}
          placeholder={placeholder}
          description="Type to search or create a new team member"
          value={search}
          onChange={(event) => {
            combobox.openDropdown();
            combobox.updateSelectedOptionIndex();
            setSearch(event.currentTarget.value);
          }}
          onClick={() => combobox.openDropdown()}
          onFocus={() => combobox.openDropdown()}
          onBlur={() => {
            combobox.closeDropdown();
            // Keep the selected user's label visible
            const selected = users.find(u => u.value === value);
            if (selected) {
              setSearch(selected.label);
            } else if (!value) {
              setSearch('');
            }
          }}
          onKeyDown={(event) => {
            if (event.key === 'Enter' && search.trim() && !exactOptionMatch && filteredOptions.length === 0) {
              event.preventDefault();
              handleCreate();
            }
          }}
          rightSection={
            value && (
              <button
                onClick={(e) => {
                  e.stopPropagation();
                  onChange?.(null);
                  setSearch('');
                }}
                className="text-gray-400 hover:text-gray-600 cursor-pointer"
                type="button"
              >
                ✕
              </button>
            )
          }
        />
      </Combobox.Target>

      <Combobox.Dropdown>
        <Combobox.Options>
          {options.length > 0 && (
            <>
              <Combobox.Group label="Existing Users">
                {options}
              </Combobox.Group>
              {!exactOptionMatch && search.trim().length > 0 && (
                <Combobox.Group label="Or create new">
                  <Combobox.Option value="$create">
                    <Group gap="xs">
                      <Text size="lg">+</Text>
                      <div>
                        <Text size="sm" fw={500}>Create "{search}"</Text>
                        <Text size="xs" c="dimmed">Add as new team member</Text>
                      </div>
                    </Group>
                  </Combobox.Option>
                </Combobox.Group>
              )}
            </>
          )}
          
          {options.length === 0 && !search.trim() && (
            <Combobox.Empty>
              <Text size="sm" c="dimmed" ta="center">
                Start typing to search users...
              </Text>
            </Combobox.Empty>
          )}
          
          {options.length === 0 && search.trim() && !exactOptionMatch && (
            <Combobox.Option value="$create">
              <Group gap="xs">
                <Text size="lg">+</Text>
                <div>
                  <Text size="sm" fw={500}>Create "{search}"</Text>
                  <Text size="xs" c="dimmed">No matching users found. Click to add as new member.</Text>
                </div>
              </Group>
            </Combobox.Option>
          )}

          {options.length === 0 && search.trim() && exactOptionMatch && (
            <Combobox.Empty>
              <Text size="sm" c="dimmed" ta="center">
                No users found
              </Text>
            </Combobox.Empty>
          )}
        </Combobox.Options>
      </Combobox.Dropdown>
    </Combobox>
  );
}