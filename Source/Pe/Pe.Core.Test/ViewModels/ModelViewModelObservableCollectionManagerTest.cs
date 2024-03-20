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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.ViewModels
{
    [TestClass]
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

        [TestMethod]
        public void Constructor_none_Test()
        {
            var cm = Create(Array.Empty<int>(), new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
            });
            Assert.AreEqual(0, cm.Model.Count);
            Assert.AreEqual(0, cm.ViewModel.Count);
        }

        [TestMethod]
        public void Constructor_some_Test()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
            });
            Assert.AreEqual(3, cm.Model.Count);
            Assert.AreEqual(3, cm.ViewModel.Count);
        }

        [TestMethod]
        public void Constructor_throw_Test()
        {
            var actual1 = Assert.ThrowsException<ArgumentNullException>(() => Create(Array.Empty<int>(), new ModelViewModelObservableCollectionOptions<Model, ViewModel>() { ToViewModel = null! }));
            Assert.AreEqual("options." + nameof(ModelViewModelObservableCollectionOptions<Model, ViewModel>.ToViewModel), actual1.ParamName);

            var actual2 = Assert.ThrowsException<ArgumentNullException>(() => Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() { ToViewModel = null! }));
            Assert.AreEqual("options." + nameof(ModelViewModelObservableCollectionOptions<Model, ViewModel>.ToViewModel), actual2.ParamName);
        }

        [TestMethod]
        public void AddTest()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
            });
            cm.Model.Add(new Model() { Value = 4 });

            Assert.AreEqual(4, cm.ViewModel.ViewModels[3].Value);
        }

        [TestMethod]
        public void AddDelegateTest()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
                AddItems = p => {
                    Assert.AreEqual(p.NewModels.Count, p.NewViewModels.Count);

                    Assert.AreEqual(4, p.NewModels[0].Value);
                    Assert.AreEqual(4, p.NewViewModels[0].Value);
                }
            });
            cm.Model.Add(new Model() { Value = 4 });

            Assert.AreEqual(4, cm.ViewModel.ViewModels[3].Value);
        }

        [TestMethod]
        public void InsertTest()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
            });
            cm.Model.Insert(1, new Model() { Value = 40 });

            Assert.AreEqual(40, cm.ViewModel.ViewModels[1].Value);
        }

        [TestMethod]
        public void InsertDelegateTest()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
                InsertItems = p => {
                    Assert.AreEqual(1, p.NewModels.Count);

                    Assert.AreEqual(1, p.InsertIndex);
                    Assert.AreEqual(40, p.NewModels[0].Value);
                }
            });
            cm.Model.Insert(1, new Model() { Value = 40 });

            Assert.AreEqual(40, cm.ViewModel.ViewModels[1].Value);
        }


        [TestMethod]
        public void RemoveTest()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
            });
            cm.Model.RemoveAt(1);

            Assert.AreEqual(2, cm.ViewModel.Count);
            Assert.AreEqual(3, cm.ViewModel.ViewModels[1].Value);
        }

        [TestMethod]
        public void RemoveDelegateTest()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
                RemoveItems = p => {
                    Assert.AreEqual(p.OldModels.Count, p.OldViewModels.Count);

                    Assert.AreEqual(1, p.OldStartingIndex);
                    Assert.AreEqual(2, p.OldModels[0].Value);
                    if(p.Apply == ModelViewModelObservableCollectionViewModelApply.Before) {
                        Assert.IsFalse(p.OldViewModels[0].IsDisposed);
                        Assert.AreEqual(2, p.OldViewModels[0].Value);
                    } else {
                        Debug.Assert(p.Apply == ModelViewModelObservableCollectionViewModelApply.After);
                        Assert.IsTrue(p.OldViewModels[0].IsDisposed);
                    }
                }
            });
            cm.Model.RemoveAt(1);

            Assert.AreEqual(2, cm.ViewModel.Count);
            Assert.AreEqual(3, cm.ViewModel.ViewModels[1].Value);
        }


        [TestMethod]
        public void RemoveDisposeTest()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
                RemoveItems = p => {
                    Assert.AreEqual(p.OldModels.Count, p.OldViewModels.Count);

                    Assert.AreEqual(1, p.OldStartingIndex);
                    Assert.AreEqual(2, p.OldModels[0].Value);
                    Assert.AreEqual(2, p.OldViewModels[0].Value);
                    Assert.IsFalse(p.OldViewModels[0].IsDisposed);
                },
                AutoDisposeViewModel = false,
            });
            cm.Model.RemoveAt(1);

            Assert.AreEqual(2, cm.ViewModel.Count);
            Assert.AreEqual(3, cm.ViewModel.ViewModels[1].Value);
        }

        [TestMethod]
        public void ReplaceTest()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
            });
            cm.Model[1] = new Model { Value = -2 };
            Assert.AreEqual(-2, cm.ViewModel.ViewModels[1].Value);
        }

        [TestMethod]
        public void ReplaceDelegateTest()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
                ReplaceItems = p => {
                    Assert.AreEqual(p.NewModels.Count, p.OldModels.Count);

                    Assert.AreEqual(1, p.StartIndex);
                    Assert.AreEqual(2, p.OldModels[0].Value);
                    Assert.AreEqual(2, p.OldViewModels[0].Value);
                    Assert.IsFalse(p.OldViewModels[0].IsDisposed);
                }
            });
            cm.Model[1] = new Model { Value = -2 };
            Assert.AreEqual(-2, cm.ViewModel.ViewModels[1].Value);
        }

        [TestMethod]
        public void MoveTest()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
            });
            cm.Model.Move(2, 0);
            Assert.AreEqual(3, cm.ViewModel.ViewModels[0].Value);
            Assert.AreEqual(2, cm.ViewModel.ViewModels[2].Value);
        }

        [TestMethod]
        public void ResetTest()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
            });
            cm.Model.Clear();
            Assert.AreEqual(0, cm.ViewModel.Count);
        }

        [TestMethod]
        public void ResetDelegateTest()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
                ResetItems = p => {
                    if(p.Apply == ModelViewModelObservableCollectionViewModelApply.Before) {
                        Assert.IsTrue(p.OldViewModels.All(a => !a.IsDisposed));
                    } else {
                        Debug.Assert(p.Apply == ModelViewModelObservableCollectionViewModelApply.After);
                        Assert.IsTrue(p.OldViewModels.All(a => a.IsDisposed));
                    }
                }
            });
            cm.Model.Clear();
            Assert.AreEqual(0, cm.ViewModel.Count);
        }

        [TestMethod]
        public void ResetDisposeTest()
        {
            var cm = Create(new int[] { 1, 2, 3 }, new ModelViewModelObservableCollectionOptions<Model, ViewModel>() {
                ToViewModel = m => new ViewModel(m, NullLoggerFactory.Instance),
                ResetItems = p => {
                    Assert.IsTrue(p.OldViewModels.All(a => !a.IsDisposed));
                }
            });
            cm.Model.Clear();
            Assert.AreEqual(0, cm.ViewModel.Count);
        }

        #endregion
    }
}
