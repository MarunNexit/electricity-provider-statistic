import { Typography, Stack, Card, CardContent } from '@mui/material';

interface GroupProvider {
    name: string;
    share: number;
}

interface Group {
    groupName: string;
    share: number;
    providers: GroupProvider[];
}

interface Props {
    groups: Group[];
    selectedGroupName: string | null;
    onSelectGroup: (name: string) => void;
}

export default function ProviderGroupsList({ groups, selectedGroupName, onSelectGroup }: Props) {
    return (
        <>
            {groups.map((group, i) => (
                <Card
                    key={i}
                    variant="outlined"
                    sx={{
                        mb: 2,
                        cursor: 'pointer',
                        backgroundColor: selectedGroupName === group.groupName ? 'primary.dark' : 'inherit',
                    }}
                    onClick={() => onSelectGroup(group.groupName)}
                >
                    <CardContent>
                        <Typography variant="subtitle1" sx={{ mb: 1 }}>
                            {group.groupName} — {group.share.toFixed(2)}%
                        </Typography>

                        {group.providers.map((provider, j) => (
                            <Stack key={j} direction="row" justifyContent="space-between" sx={{ mb: 0.5 }}>
                                <Typography variant="body2">{provider.name}</Typography>
                                <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                                    {provider.share.toFixed(2)}%
                                </Typography>
                            </Stack>
                        ))}
                    </CardContent>
                </Card>
            ))}
        </>
    );
}
