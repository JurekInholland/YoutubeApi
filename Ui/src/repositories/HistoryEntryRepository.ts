import { Repository } from 'pinia-orm'
import HistoryEntry from '@/models/HistoryEntry'

export default class HistoryEntryRepository extends Repository {
  use = HistoryEntry

  public setProgress(entry: HistoryEntry) {
    this.save(entry)
  }
  public clearHistory() {
    this.flush()
  }

  public updateProgress(id: string, progress: number) {
    const entry = this.find(id)
    if (entry) {
      entry.progress = progress
      this.save(entry)
    }
  }
}
