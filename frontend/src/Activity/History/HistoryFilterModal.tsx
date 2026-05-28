import React, { useCallback } from 'react';
import FilterModal, { FilterModalProps } from 'Components/Filter/FilterModal';
import { setHistoryOption } from './historyOptionsStore';
import useHistory, { FILTER_BUILDER } from './useHistory';

type HistoryFilterModalProps = FilterModalProps<History>;

export default function HistoryFilterModal(props: HistoryFilterModalProps) {
  const { records } = useHistory();

  const dispatchSetFilter = useCallback(
    ({ selectedFilterKey }: { selectedFilterKey: string | number }) => {
      setHistoryOption('selectedFilterKey', selectedFilterKey);
    },
    []
  );

  return (
    <FilterModal
      {...props}
      sectionItems={records}
      filterBuilderProps={FILTER_BUILDER}
      customFilterType="history"
      dispatchSetFilter={dispatchSetFilter}
    />
  );
}
