/**
 * Formats a number with number separators
 */
export const addSeperators = (num: number, separator: string = ','): string => {
  return num.toLocaleString(undefined, {
    useGrouping: true,
    maximumFractionDigits: 0,
    style: 'decimal'
  })
}

export const formatViews = (views: number): string => {
  const suffixes: { [key: number]: string } = {
    1000000000000: 'T', // save for the future :)
    1000000000: 'B',
    1000000: 'M',
    1000: 'K'
  }

  for (const magnitude of Object.keys(suffixes) as unknown as number[]) {
    if (views >= magnitude) {
      return (views / magnitude).toFixed(1) + suffixes[magnitude]
    }
  }

  return views.toString()
}

export const formatDateAgo = (date: Date): string => {
  const seconds = Math.floor((Date.now() - date.getTime()) / 1000)
  const intervals: { [key: string]: number } = {
    year: 31536000,
    month: 2592000,
    week: 604800,
    day: 86400,
    hour: 3600,
    minute: 60
  }

  for (const interval of Object.keys(intervals)) {
    const divisor = intervals[interval]
    if (seconds >= divisor) {
      const count = Math.floor(seconds / divisor)
      return `${count} ${interval}${count > 1 ? 's' : ''} ago`
    }
  }
  return 'just now'
}
