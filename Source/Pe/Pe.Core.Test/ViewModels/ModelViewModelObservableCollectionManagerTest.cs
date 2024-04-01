using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.ViewModels
{
    public class ModelViewModelObservableCollectionManagerTest
    {
        #region define

        private class Model: NotifyPropertyBase
        {
            #region property

            public int Value { get; set; }

            #endregion
        }

        private class ViewModel: SingleModelViewModelBase<Model>
        {
            public ViewModel(Model model, ILoggerFactory loggerFactory)
                : base(model, loggerFactory)
            { }

            public int Value
            {
                get => Model.Value;
                set => SetModelValue(value);
            }

            public int BigValue
            {
                get => Value * 100;
                set => SetModelValue(value / 100, nameof(Model.Value));
            }
        }

        private class Collections
        {
            public Collections(IEnumerable<int> source, ModelViewModelObservableCollectionOptions<Model, ViewModel> options)
            {
                Model = new ObservableCollection<Model>(source.Select(a => new Model() { Value = a, }));
                ViewModel = new ModelViewModelObservableCollectionManager<Model, ViewModel>(Model, options);
            }

            public ObservableCollection<Model> Model { get; }
            public ModelViewModelObservableCollectionManager<Model, ViewModel> ViewModel { get; }
        }

        #endregion

        #region function


        private Collections Create(int[] args, ModelViewModelObservableCollectionOptions<Model, ViewModel> options)
        {
            return new Collections(args, options);
        }

        [Fact]
        public void Constructor_none_Test()
        {
            var cm = Create(Array.Empty<int>(), new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
            });
            Assert.Empty(cm.Model);
            Assert.Equal(0, cm.ViewModel.Count);
        }

        [Fact]
        public void Constructor_some_Test()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
            });
            Assert.Equal(3, cm.Model.Count);
            Assert.Equal(3, cm.ViewModel.Count);
        }

        [Fact]
        public void Constructor_throw_Test()
        {
            var actual1 = Assert.Throws<ArgumentNullException>(() => Create(Array.Empty<int>(), new ModelViewModelObservableCollectionOptions<Model, ViewModel>() { ToViewModel = null! }));
            Assert.Equal("options." + nameof(ModelViewModelObservableCollectionOptions<Model, ViewModel>.ToViewModel), actual1.ParamName);

            var actual2 = Assert.Throws<ArgumentNullException>(() => Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() { ToViewModel = null! }));
            Assert.Equal("options." + nameof(ModelViewModelObservableCollectionOptions<Model, ViewModel>.ToViewModel), actual2.ParamName);
        }


        [Fact]
        public void IndexOfTest()
        {
            var test = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
            });
            Assert.Equal(1, test.ViewModel.IndexOf(test.ViewModel.ViewModels[1]));
            Assert.Equal(-1, test.ViewModel.IndexOf(new Model() { Value = 4 }));
        }

        [Fact]
        public void TryGetModelTest()
        {
            var test = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
            });

            Assert.True(test.ViewModel.TryGetModel(test.ViewModel.ViewModels[1], out var actual));
            Assert.Equal(2, actual.Value);

            Assert.False(test.ViewModel.TryGetModel(new ViewModel(new Model() { Value = 4 }, NullLoggerFactory.Instance), out var _));
        }

        [Fact]
        public void TryGetViewModelTest()
        {
            var test = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
            });

            Assert.True(test.ViewModel.TryGetViewModel(test.Model[1], out var actual));
            Assert.Equal(2, actual.Value);

            Assert.False(test.ViewModel.TryGetViewModel(new Model() { Value = 4 }, out var _));
        }

        [Fact]
        public void AddTest()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
            });
            cm.Model.Add(new Model() { Value = 4 });

            Assert.Equal(4, cm.ViewModel.ViewModels[3].Value);
        }

        [Fact]
        public void AddDelegateTest()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
                AddItems = p => {
                    Assert.Equal(p.NewModels.Count, p.NewViewModels.Count);

                    Assert.Equal(4, p.NewModels[0].Value);
                    Assert.Equal(4, p.NewViewModels[0].Value);
                }
            });
            cm.Model.Add(new Model() { Value = 4 });

            Assert.Equal(4, cm.ViewModel.ViewModels[3].Value);
        }

        [Fact]
        public void InsertTest()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
            });
            cm.Model.Insert(1, new Model() { Value = 40 });

            Assert.Equal(40, cm.ViewModel.ViewModels[1].Value);
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Assertions", "xUnit2013:Do not use equality check to check for collection size.", Justification = "<保留中>")]
        public void InsertDelegateTest()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
                InsertItems = p => {
                    Assert.Equal(1, p.NewModels.Count);

                    Assert.Equal(1, p.InsertIndex);
                    Assert.Equal(40, p.NewModels[0].Value);
                }
            });
            cm.Model.Insert(1, new Model() { Value = 40 });

            Assert.Equal(40, cm.ViewModel.ViewModels[1].Value);
        }


        [Fact]
        public void RemoveTest()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
            });
            cm.Model.RemoveAt(1);

            Assert.Equal(2, cm.ViewModel.Count);
            Assert.Equal(3, cm.ViewModel.ViewModels[1].Value);
        }

        [Fact]
        public void RemoveDelegateTest()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
                RemoveItems = p => {
                    Assert.Equal(p.OldModels.Count, p.OldViewModels.Count);

                    Assert.Equal(1, p.OldStartingIndex);
                    Assert.Equal(2, p.OldModels[0].Value);
                    if(p.Apply == ModelViewModelObservableCollectionViewModelApply.Before) {
                        Assert.False(p.OldViewModels[0].IsDisposed);
                        Assert.Equal(2, p.OldViewModels[0].Value);
                    } else {
                        Debug.Assert(p.Apply == ModelViewModelObservableCollectionViewModelApply.After);
                        Assert.True(p.OldViewModels[0].IsDisposed);
                        Assert.Throws<NullReferenceException>(() => p.OldViewModels[0].Value); // モデルには null が強制設定されている
                    }
                }
            });
            cm.Model.RemoveAt(1);

            Assert.Equal(2, cm.ViewModel.Count);
            Assert.Equal(3, cm.ViewModel.ViewModels[1].Value);
        }


        [Fact]
        public void RemoveDisposeTest()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
                RemoveItems = p => {
                    Assert.Equal(p.OldModels.Count, p.OldViewModels.Count);

                    Assert.Equal(1, p.OldStartingIndex);
                    Assert.Equal(2, p.OldModels[0].Value);
                    Assert.Equal(2, p.OldViewModels[0].Value);
                    Assert.False(p.OldViewModels[0].IsDisposed);
                },
                AutoDisposeViewModel = false,
            });
            cm.Model.RemoveAt(1);

            Assert.Equal(2, cm.ViewModel.Count);
            Assert.Equal(3, cm.ViewModel.ViewModels[1].Value);
        }

        [Fact]
        public void ReplaceTest()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
            });
            cm.Model[1] = new Model { Value = -2 };
            Assert.Equal(-2, cm.ViewModel.ViewModels[1].Value);
        }

        [Fact]
        public void ReplaceDelegateTest()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
                ReplaceItems = p => {
                    Assert.Equal(p.NewModels.Count, p.OldModels.Count);

                    Assert.Equal(1, p.StartIndex);
                    Assert.Equal(2, p.OldModels[0].Value);
                    Assert.Equal(2, p.OldViewModels[0].Value);
                    Assert.False(p.OldViewModels[0].IsDisposed);
                }
            });
            cm.Model[1] = new Model { Value = -2 };
            Assert.Equal(-2, cm.ViewModel.ViewModels[1].Value);
        }

        [Fact]
        public void MoveTest()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
            });
            cm.Model.Move(2, 0);
            Assert.Equal(3, cm.ViewModel.ViewModels[0].Value);
            Assert.Equal(2, cm.ViewModel.ViewModels[2].Value);
        }

        [Fact]
        public void ResetTest()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
            });
            cm.Model.Clear();
            Assert.Equal(0, cm.ViewModel.Count);
        }

        [Fact]
        public void ResetDelegateTest()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
                ResetItems = p => {
                    if(p.Apply == ModelViewModelObservableCollectionViewModelApply.Before) {
                        Assert.True(p.OldViewModels.All(a => !a.IsDisposed));
                    } else {
                        Debug.Assert(p.Apply == ModelViewModelObservableCollectionViewModelApply.After);
                        Assert.True(p.OldViewModels.All(a => a.IsDisposed));
                    }
                }
            });
            cm.Model.Clear();
            Assert.Equal(0, cm.ViewModel.Count);
        }

        [Fact]
        public void ResetDisposeTest()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
                ResetItems = p => {
                    Assert.True(p.OldViewModels.All(a => !a.IsDisposed));
                }
            });
            cm.Model.Clear();
            Assert.Equal(0, cm.ViewModel.Count);
        }

        #endregion
    }
}
