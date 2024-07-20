export function formatDate(dateIsoString: string): string {
  const date = new Date(dateIsoString);
  const formatted = new Intl.DateTimeFormat('en-GB', { dateStyle: 'full' }).format(date);

  return formatted;
}

export function formatAsPercentage(value: number): string {
  const formatter = Intl.NumberFormat('en-US', {
    style: 'percent'
  });
  const percent = formatter.format(value);

  return percent;
}

export function getShortGuid(guid: string): string {
  return guid.substring(0, 8);
}
